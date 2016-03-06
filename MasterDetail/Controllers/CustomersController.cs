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
    public class CustomersController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public async Task<ActionResult> Index()
        {
            return View(await _applicationDbContext.Customers.Where(c => c.Cloaked == false).ToListAsync());
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await _applicationDbContext.Customers.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerId,AccountNumber,CompanyName,Address,City,State,ZipCode,Phone,Cloaked")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Customers.Add(customer);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(customer);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await _applicationDbContext.Customers.FindAsync(id);

            if (customer.Cloaked)
                return RedirectToAction("Index");

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CustomerId,AccountNumber,CompanyName,Address,City,State,ZipCode,Phone,Cloaked")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(customer).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = await _applicationDbContext.Customers.FindAsync(id);

            if (customer.Cloaked)
                return RedirectToAction("Index");

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Customer customer = await _applicationDbContext.Customers.FindAsync(id);
            //_applicationDbContext.Customers.Remove(customer);
            customer.Cloaked = true;
            _applicationDbContext.Entry(customer).State = EntityState.Modified;
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
