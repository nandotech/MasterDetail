using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterDetail.DataLayer;
using MasterDetail.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MasterDetail.Controllers
{
    [Authorize]
    public class WorkOrdersController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public async Task<ActionResult> Index()
        {
            var workOrders = _applicationDbContext.WorkOrders.Include(w => w.CurrentWorker).Include(w => w.Customer);
            return View(await workOrders.ToListAsync());
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await _applicationDbContext.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            return View(workOrder);
        }


        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(_applicationDbContext.Customers.Where(c => c.Cloaked == false), "CustomerId", "CompanyName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "WorkOrderId,CustomerId,OrderDateTime,TargetDateTime,DropDeadDateTime,Description,WorkOrderStatus,CertificationRequirements,CurrentWorkerId")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                workOrder.CurrentWorkerId = User.Identity.GetUserId();
                _applicationDbContext.WorkOrders.Add(workOrder);
                await _applicationDbContext.SaveChangesAsync();

                Log4NetHelper.Log(String.Format("Work order {0} created", workOrder.WorkOrderId), LogLevel.INFO, workOrder.EntityFormalNamePlural, workOrder.WorkOrderId, User.Identity.Name, null);

                return RedirectToAction("Edit", new { controller = "WorkOrders", action = "Edit", Id = workOrder.WorkOrderId});
            }

            ViewBag.CustomerId = new SelectList(_applicationDbContext.Customers.Where(c => c.Cloaked == false), "CustomerId", "AccountNumber", workOrder.CustomerId);
            return View(workOrder);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await _applicationDbContext.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }

            // If a different user has claimed the work order since you refreshed the work list, redirect to work list with error message.
            if (workOrder.CurrentWorkerId != null && workOrder.CurrentWorkerId != User.Identity.GetUserId())
            {
                ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string claimedUserName = userManager.FindById(workOrder.CurrentWorkerId).UserName;

                string message = String.Format("User {0} has claimed work order {1} before user {2} could, and so the work order remains claimed by {0}", claimedUserName, workOrder.WorkOrderId, User.Identity.GetUserName());

                TempData["MessageToClient"] = message;

                Log4NetHelper.Log(message, LogLevel.INFO, workOrder.EntityFormalNamePlural, workOrder.WorkOrderId, User.Identity.Name, null);

                return RedirectToAction("Index", "WorkList");
            }

            if (workOrder.Status.Substring(workOrder.Status.Length - 3, 3) != "ing")
                return View("Claim", workOrder);

            return View(workOrder.Status, workOrder);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "WorkOrderId,CustomerId,OrderDateTime,TargetDateTime,DropDeadDateTime,Description,WorkOrderStatus,CertificationRequirements,CurrentWorkerId,ReworkNotes,RowVersion")] WorkOrder workOrder, string command)
        {
            if (ModelState.IsValid)
            {
                // Populate Parts and Labors
                workOrder.Parts = _applicationDbContext.Parts.Where(p => p.WorkOrderId == workOrder.WorkOrderId).ToList();
                workOrder.Labors = _applicationDbContext.Labors.Where(l => l.WorkOrderId == workOrder.WorkOrderId).ToList();

                PromotionResult promotionResult = new PromotionResult();

                if (command == "Save")
                {
                    promotionResult.Success = true;
                    promotionResult.Message = String.Format("Changes to work order {0} have been successfully saved.", workOrder.Id);
                    Log4NetHelper.Log(promotionResult.Message, LogLevel.INFO, workOrder.EntityFormalNamePlural, workOrder.Id, User.Identity.Name, null);
                }
                else if (command == "Claim")
                    promotionResult = workOrder.ClaimWorkListItem(User.Identity.GetUserId());
                else if (command == "Relinquish")
                    promotionResult = workOrder.RelinquishWorkListItem();
                else
                    promotionResult = workOrder.PromoteWorkListItem(command);

                if (!promotionResult.Success)
                    TempData["MessageToClient"] = promotionResult.Message;

                _applicationDbContext.Entry(workOrder).State = EntityState.Modified;

                try
                {
                    await _applicationDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (command == "Claim")
                        TempData["MessageToClient"] = String.Format("Someone else has claimed work order {0} since you retrieved it.", workOrder.WorkOrderId);
                    else
                        TempData["MessageToClient"] = String.Format("Someone else has modified work order {0} since you retrieved it.  Your changes have not been applied.", workOrder.WorkOrderId);

                    return RedirectToAction("Index", "WorkList");
                }

                if (command == "Claim" && promotionResult.Success)
                    return RedirectToAction("Edit", workOrder.WorkOrderId);

                return RedirectToAction("Index", "WorkList");
            }

            return View(workOrder);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkOrder workOrder = await _applicationDbContext.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return HttpNotFound();
            }
            return View(workOrder);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkOrder workOrder = await _applicationDbContext.WorkOrders.FindAsync(id);
            _applicationDbContext.WorkOrders.Remove(workOrder);
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _applicationDbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
