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
    public class PartsController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public async Task<ActionResult> Index(int workOrderId, bool? readOnly)
        {
            ViewBag.WorkOrderId = workOrderId;
            var parts = _applicationDbContext.Parts
                .Where(p => p.WorkOrderId == workOrderId)
                .OrderBy(p => p.InventoryItemCode);

            string partialViewName = readOnly == true ? "_IndexReadOnly" : "_Index";
            return PartialView(partialViewName, await parts.ToListAsync());
        }


        public ActionResult Create(int workOrderId)
        {
            Part part = new Part();
            part.WorkOrderId = workOrderId;
            return PartialView("_Create", part);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PartId,WorkOrderId,InventoryItemCode,InventoryItemName,Quantity,UnitPrice,Notes,IsInstalled")] Part part)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Parts.Add(part);
                await _applicationDbContext.SaveChangesAsync();

                Log4NetHelper.Log(String.Format("Part {0} has been added to work order {1} for {2} at ${3:#,###.00} each.", part.InventoryItemCode, part.WorkOrderId, part.Quantity, part.UnitPrice), LogLevel.INFO, "WorkOrders", part.WorkOrderId, User.Identity.Name, null);

                return Json(new {success = true});
            }

            return PartialView("_Create", part);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = await _applicationDbContext.Parts.FindAsync(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Edit", part);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PartId,WorkOrderId,InventoryItemCode,InventoryItemName,Quantity,UnitPrice,Notes,IsInstalled")] Part part)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(part).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();

                Log4NetHelper.Log(String.Format("Part {0} has been updated in work order {1} to {2} at ${3:#,###.00} each.", part.InventoryItemCode, part.WorkOrderId, part.Quantity, part.UnitPrice), LogLevel.INFO, "WorkOrders", part.WorkOrderId, User.Identity.Name, null);

                return Json(new { success = true });
            }
            return PartialView("_Edit", part);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = await _applicationDbContext.Parts.FindAsync(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete", part);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Part part = await _applicationDbContext.Parts.FindAsync(id);
            _applicationDbContext.Parts.Remove(part);
            await _applicationDbContext.SaveChangesAsync();

            Log4NetHelper.Log(String.Format("Part {0} has been deleted from work order {1}.", part.InventoryItemCode, part.WorkOrderId), LogLevel.INFO, "WorkOrders", part.WorkOrderId, User.Identity.Name, null);

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
