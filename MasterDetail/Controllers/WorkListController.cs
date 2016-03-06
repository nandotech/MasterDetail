using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MasterDetail.DataLayer;
using MasterDetail.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MasterDetail.Controllers
{
    [Authorize]
    public class WorkListController : Controller
    {
        ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }


        private IEnumerable<IWorkListItem> GetWorkOrders(string userId, List<string> userRolesList)
        {
            IEnumerable<IWorkListItem> claimableWorkOrders = _applicationDbContext.WorkOrders.Where(
                wo => wo.WorkOrderStatus != WorkOrderStatus.Approved)
                .ToList()
                .Where(
                    wo => userRolesList.Any(ur => wo.RolesWhichCanClaim.Contains(ur)));

            IEnumerable<IWorkListItem> workOrdersIAmWOrkingOn = _applicationDbContext.WorkOrders.Where(
                wo => wo.CurrentWorkerId == userId);

            return claimableWorkOrders.Concat(workOrdersIAmWOrkingOn);
        }


        private IEnumerable<IWorkListItem> GetWidgets(string userId, List<string> userRolesList)
        {
            IEnumerable<IWorkListItem> claimableWidgets = _applicationDbContext.Widgets.Where(
                w => w.WidgetStatus != WidgetStatus.Approved)
                .ToList()
                .Where(
                    wo => userRolesList.Any(ur => wo.RolesWhichCanClaim.Contains(ur)));

            IEnumerable<IWorkListItem> widgetsIAmWorkingOn =
                _applicationDbContext.Widgets.Where(w => w.CurrentWorkerId == userId);

            return claimableWidgets.Concat(widgetsIAmWorkingOn);
        }


        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<string> userRolesList = UserManager.GetRoles(userId).ToList();

            IEnumerable<IWorkListItem> workListItemsToDisplay = new List<IWorkListItem>();
            workListItemsToDisplay = workListItemsToDisplay.Concat(GetWorkOrders(userId, userRolesList));
            workListItemsToDisplay = workListItemsToDisplay.Concat(GetWidgets(userId, userRolesList));

            return View(workListItemsToDisplay.OrderByDescending(wl => wl.PriorityScore));
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