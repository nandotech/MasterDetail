using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterDetail.DataLayer;
using MasterDetail.Models;
using Microsoft.AspNet.Identity;

namespace MasterDetail.Controllers
{
    [Authorize]
    public class WidgetsController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public ActionResult Index()
        {
            var widgets = _applicationDbContext.Widgets;
            return View(widgets.ToList());
        }


        public ActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WidgetId,Description,MainBusCode,TestPassDateTime,WidgetStatus,CurrentWorkerId")] Widget widget)
        {
            if (ModelState.IsValid)
            {
                // Do not assign a user because the WidgetStatus is Created from the very beginning of its lifecycle
                _applicationDbContext.Widgets.Add(widget);
                _applicationDbContext.SaveChanges();

                Log4NetHelper.Log(String.Format("Widget {0} created", widget.WidgetId), LogLevel.INFO, widget.EntityFormalNamePlural, widget.WidgetId, User.Identity.Name, null);

                return RedirectToAction("Index", "WorkList");
            }

            return View(widget);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Widget widget = _applicationDbContext.Widgets.Find(id);
            if (widget == null)
            {
                return HttpNotFound();
            }

            if (widget.Status.Substring(widget.Status.Length - 3, 3) != "ing")
                return View("Claim", widget);

            return View(widget.Status, widget);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WidgetId,Description,MainBusCode,TestPassDateTime,WidgetStatus,CurrentWorkerId")] Widget widget, string command)
        {
            if (ModelState.IsValid)
            {
                PromotionResult promotionResult = new PromotionResult();

                if (command == "Save")
                {
                    promotionResult.Success = true;
                    promotionResult.Message = String.Format("Changes to widget {0} have been successfully saved.", widget.WidgetId);
                    Log4NetHelper.Log(promotionResult.Message, LogLevel.INFO, widget.EntityFormalNamePlural, widget.WidgetId, User.Identity.Name, null);
                }
                else if (command == "Claim")
                    promotionResult = widget.ClaimWorkListItem(User.Identity.GetUserId());
                else if (command == "Relinquish")
                    promotionResult = widget.RelinquishWorkListItem();
                else
                    promotionResult = widget.PromoteWorkListItem(command);

                if (!promotionResult.Success)
                    TempData["MessageToClient"] = promotionResult.Message;

                _applicationDbContext.Entry(widget).State = EntityState.Modified;
                _applicationDbContext.SaveChanges();

                if (command == "Claim" && promotionResult.Success)
                    return RedirectToAction("Edit", widget.WidgetId);

                return RedirectToAction("Index", "WorkList");
            }

            return View(widget);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Widget widget = _applicationDbContext.Widgets.Find(id);
            if (widget == null)
            {
                return HttpNotFound();
            }
            return View(widget);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Widget widget = _applicationDbContext.Widgets.Find(id);
            _applicationDbContext.Widgets.Remove(widget);
            _applicationDbContext.SaveChanges();
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
