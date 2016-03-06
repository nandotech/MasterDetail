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
    public class ServiceItemsController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        // GET: ServiceItems
        public async Task<ActionResult> Index()
        {
            return View(await _applicationDbContext.ServiceItems.ToListAsync());
        }

        // GET: ServiceItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceItem serviceItem = await _applicationDbContext.ServiceItems.FindAsync(id);
            if (serviceItem == null)
            {
                return HttpNotFound();
            }
            return View(serviceItem);
        }

        // GET: ServiceItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ServiceItemId,ServiceItemCode,ServiceItemName,Rate")] ServiceItem serviceItem)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.ServiceItems.Add(serviceItem);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(serviceItem);
        }

        // GET: ServiceItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceItem serviceItem = await _applicationDbContext.ServiceItems.FindAsync(id);
            if (serviceItem == null)
            {
                return HttpNotFound();
            }
            return View(serviceItem);
        }

        // POST: ServiceItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceItemId,ServiceItemCode,ServiceItemName,Rate")] ServiceItem serviceItem)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(serviceItem).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(serviceItem);
        }

        // GET: ServiceItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceItem serviceItem = await _applicationDbContext.ServiceItems.FindAsync(id);
            if (serviceItem == null)
            {
                return HttpNotFound();
            }
            return View(serviceItem);
        }

        // POST: ServiceItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiceItem serviceItem = await _applicationDbContext.ServiceItems.FindAsync(id);
            _applicationDbContext.ServiceItems.Remove(serviceItem);
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


        public JsonResult GetServiceItemsForAutocomplete(string term)
        {
            ServiceItem[] matchingServiceItems = String.IsNullOrWhiteSpace(term)
                ? null
                : _applicationDbContext.ServiceItems.Where(
                    si =>
                        si.ServiceItemCode.Contains(term) ||
                        si.ServiceItemName.Contains(term)).ToArray();

            return Json(matchingServiceItems.Select(m => new
            {
                id = m.ServiceItemCode,
                value = m.ServiceItemCode,
                label = String.Format("{0}: {1}", m.ServiceItemCode, m.ServiceItemName),
                ServiceItemName = m.ServiceItemName,
                Rate = m.Rate
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
