using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterDetail.DataLayer;
using MasterDetail.Models;

namespace MasterDetail.Controllers
{
    public class LaborsController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public async Task<ActionResult> Index(int workOrderId, bool? readOnly)
        {
            ViewBag.WorkOrderId = workOrderId;
            var labors = _applicationDbContext.Labors
                .Where(l => l.WorkOrderId == workOrderId)
                .OrderBy(l => l.ServiceItemCode);

            string partialViewName = readOnly == true ? "_IndexReadOnly" : "_Index";
            return PartialView(partialViewName, await labors.ToListAsync());
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await _applicationDbContext.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            return View(labor);
        }


        public ActionResult Create(int workOrderId)
        {
            Labor labor = new Labor();
            labor.WorkOrderId = workOrderId;
            return PartialView("_Create", labor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LaborId,WorkOrderId,ServiceItemCode,ServiceItemName,LaborHours,Rate,PercentComplete,Notes")] Labor labor)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Labors.Add(labor);
                await _applicationDbContext.SaveChangesAsync();

                Log4NetHelper.Log(String.Format("Labor item {0} has been added to work order {1} for {2} hours at ${3:#,###.00}/hr.", labor.ServiceItemCode, labor.WorkOrderId, labor.LaborHours, labor.Rate), LogLevel.INFO, "WorkOrders", labor.WorkOrderId, User.Identity.Name, null);

                return Json(new { success = true });
            }

            return PartialView("_Create", labor);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await _applicationDbContext.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", labor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "LaborId,WorkOrderId,ServiceItemCode,ServiceItemName,LaborHours,Rate,PercentComplete,Notes")] Labor labor)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(labor).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();

                Log4NetHelper.Log(String.Format("Labor item {0} has been updated in work order {1} to {2} hours at ${3:#,###.00}/hr.", labor.ServiceItemCode, labor.WorkOrderId, labor.LaborHours, labor.Rate), LogLevel.INFO, "WorkOrders", labor.WorkOrderId, User.Identity.Name, null);

                return Json(new { success = true });
            }
            return PartialView("_Edit", labor);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Labor labor = await _applicationDbContext.Labors.FindAsync(id);
            if (labor == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", labor);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Labor labor = await _applicationDbContext.Labors.FindAsync(id);
            _applicationDbContext.Labors.Remove(labor);
            await _applicationDbContext.SaveChangesAsync();

            Log4NetHelper.Log(String.Format("Labor item {0} has been deleted from work order {1}.", labor.ServiceItemCode, labor.WorkOrderId), LogLevel.INFO, "WorkOrders", labor.WorkOrderId, User.Identity.Name, null);

            return Json(new { success = true });
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
