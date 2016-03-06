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
using PagedList;

namespace MasterDetail.Controllers
{
    public class InventoryItemsController : Controller
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public ActionResult Index(string sort, string search, int? page)
        {
            ViewBag.CategorySort = String.IsNullOrEmpty(sort) ? "category_desc" : string.Empty;
            ViewBag.ItemCodeSort = sort == "itemcode" ? "itemcode_desc" : "itemcode";
            ViewBag.NameSort = sort == "name" ? "name_desc" : "name";
            ViewBag.UnitPriceSort = sort == "unitprice" ? "unitprice_desc" : "unitprice";

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentSearch = search;

            IQueryable<InventoryItem> inventoryItems = _applicationDbContext.InventoryItems.Include(i => i.Category);

            if (!String.IsNullOrEmpty(search))
                inventoryItems =
                    inventoryItems.Where(
                        ii => ii.InventoryItemCode.StartsWith(search) || ii.InventoryItemName.StartsWith(search));

            switch (sort)
            {
                case "category_desc":
                    inventoryItems =
                        inventoryItems
                            .OrderByDescending(ii => ii.Category.CategoryName)
                            .ThenBy(ii => ii.InventoryItemName);
                    break;

                case "itemcode":
                    inventoryItems = inventoryItems.OrderBy(ii => ii.InventoryItemCode);
                    break;

                case "itemcode_desc":
                    inventoryItems = inventoryItems.OrderByDescending(ii => ii.InventoryItemCode);
                    break;

                case "name":
                    inventoryItems = inventoryItems.OrderBy(ii => ii.InventoryItemName);
                    break;

                case "name_desc":
                    inventoryItems = inventoryItems.OrderByDescending(ii => ii.InventoryItemName);
                    break;

                case "unitprice":
                    inventoryItems = inventoryItems.OrderBy(ii => ii.UnitPrice).ThenBy(ii => ii.InventoryItemName);;
                    break;

                case "unitprice_desc":
                    inventoryItems = inventoryItems.OrderByDescending(ii => ii.UnitPrice).ThenBy(ii => ii.InventoryItemName);;
                    break;

                default:
                    inventoryItems =
                        inventoryItems
                            .OrderBy(ii => ii.Category.CategoryName)
                            .ThenBy(ii => ii.InventoryItemName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = page ?? 1;

            return View(inventoryItems.ToPagedList(pageNumber, pageSize));
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = await _applicationDbContext.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }


        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InventoryItemId,InventoryItemCode,InventoryItemName,UnitPrice,CategoryId")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.InventoryItems.Add(inventoryItem);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName", inventoryItem.CategoryId);
            return View(inventoryItem);
        }


        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = await _applicationDbContext.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName", inventoryItem.CategoryId);
            return View(inventoryItem);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InventoryItemId,InventoryItemCode,InventoryItemName,UnitPrice,CategoryId")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.Entry(inventoryItem).State = EntityState.Modified;
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_applicationDbContext.Categories, "Id", "CategoryName", inventoryItem.CategoryId);
            return View(inventoryItem);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryItem inventoryItem = await _applicationDbContext.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItem);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InventoryItem inventoryItem = await _applicationDbContext.InventoryItems.FindAsync(id);
            _applicationDbContext.InventoryItems.Remove(inventoryItem);
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


        public JsonResult GetInventoryItemsForAutocomplete(string term)
        {
            InventoryItem[] matchingInventoryItems = String.IsNullOrWhiteSpace(term)
                ? null
                : _applicationDbContext.InventoryItems.Where(
                    ii =>
                        ii.InventoryItemCode.Contains(term) ||
                        ii.InventoryItemName.Contains(term)).ToArray();

            return Json(matchingInventoryItems.Select(m => new
            {
                id = m.InventoryItemCode,
                value = m.InventoryItemCode,
                label = String.Format("{0}: {1}", m.InventoryItemCode, m.InventoryItemName),
                InventoryItemName = m.InventoryItemName,
                UnitPrice = m.UnitPrice
            }), JsonRequestBehavior.AllowGet);
        }
    }
}
