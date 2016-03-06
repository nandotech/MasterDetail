using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasterDetail.Models
{
    public class Widget : IWorkListItem
    {
        public Widget()
        {
            WidgetStatus = WidgetStatus.Created;
        }


        public int WidgetId { get; set; }
        
        [Required(ErrorMessage = "You must enter a description.")]
        [StringLength(256, ErrorMessage = "The description must be 256 characters or shorter.")]
        public string Description { get; set; }
        
        [Display(Name = "Main Bus Code")]
        [StringLength(12, ErrorMessage = "Main Bus Code must be 12 characters or shorter.")]
        public string MainBusCode { get; set; }
        
        [Display(Name = "Test Pass Date")]
        public DateTime? TestPassDateTime { get; set; }
        
        [Display(Name = "Status")]
        public WidgetStatus WidgetStatus { get; set; }
        
        public virtual ApplicationUser CurrentWorker { get; set; }
        
        public string CurrentWorkerId { get; set; }

        public int Id
        {
            get { return WidgetId; }
        }

        public string Status
        {
            get { return WidgetStatus.ToString(); }
        }

        public string CurrentWorkerName
        {
            get
            {
                if (CurrentWorker == null)
                    return String.Empty;

                return CurrentWorker.FullName;
            }
        }

        public string EntityFamiliarName
        {
            get { return "Widget"; }
        }

        public string EntityFamiliarNamePlural
        {
            get { return "Widgets"; }
        }

        public string EntityFormalName
        {
            get { return "Widget"; }
        }

        public string EntityFormalNamePlural
        {
            get { return "Widgets"; }
        }

        public int PriorityScore
        {
            get
            {
                int priorityScore = (int) WidgetStatus;
                priorityScore += Description.Length / 10;
                if (priorityScore < 0) priorityScore = 0;
                if (priorityScore > 100) priorityScore = 100;

                return priorityScore;
            }
        }

        public IEnumerable<string> RolesWhichCanClaim
        {
            get
            {
                List<string> rolesWhichCanClaim = new List<string>();

                switch (WidgetStatus)
                {
                    case WidgetStatus.Created:
                        rolesWhichCanClaim.Add("Manager");
                        rolesWhichCanClaim.Add("Admin");
                        break;

                    case WidgetStatus.Integrated:
                        rolesWhichCanClaim.Add("Admin");
                        break;
                }

                return rolesWhichCanClaim;
            }
        }

        public PromotionResult ClaimWorkListItem(string userId)
        {
            PromotionResult promotionResult = WorkListBusinessRules.CanClaimWorkListItem(userId);

            if (!promotionResult.Success)
            {
                Log4NetHelper.Log(promotionResult.Message, LogLevel.WARN, EntityFormalNamePlural, WidgetId, HttpContext.Current.User.Identity.Name, null);
                return promotionResult;
            }

            switch (WidgetStatus)
            {
                case WidgetStatus.Created:
                    promotionResult = PromoteToIntegrating();
                    break;

                case WidgetStatus.Integrated:
                    promotionResult = PromoteToApproving();
                    break;
            }

            if (promotionResult.Success)
                CurrentWorkerId = userId;

            Log4NetHelper.Log(promotionResult.Message, LogLevel.INFO, EntityFormalNamePlural, WidgetId, HttpContext.Current.User.Identity.Name, null);

            return promotionResult;
        }

        public PromotionResult PromoteWorkListItem(string command)
        {
            PromotionResult promotionResult = new PromotionResult();

            switch (command)
            {
                case "PromoteToIntegrated":
                    promotionResult = PromoteToIntegrated();
                    break;

                case "PromoteToApproved":
                    promotionResult = PromoteToApproved();
                    break;

                case "DemoteToCanceled":
                    promotionResult = DemoteToCanceled();
                    break;
            }

            Log4NetHelper.Log(promotionResult.Message, LogLevel.INFO, EntityFormalNamePlural, WidgetId, HttpContext.Current.User.Identity.Name, null);

            if (promotionResult.Success)
            {
                CurrentWorker = null;
                CurrentWorkerId = null;
            }

            return promotionResult;
        }

        public PromotionResult RelinquishWorkListItem()
        {
            PromotionResult promotionResult = new PromotionResult { Success = true };

            if (CurrentWorkerId == null || Status.Substring(Status.Length - 3, 3) != "ing")
            {
                promotionResult.Success = false;
                promotionResult.Message = String.Format("Widget {0} cannot be relinquished because it is not currently being worked on.", WidgetId);
            }

            if (promotionResult.Success)
            {
                CurrentWorker = null;
                CurrentWorkerId = null;

                switch (WidgetStatus)
                {
                    case WidgetStatus.Integrating:
                        WidgetStatus = WidgetStatus.Created;
                        break;

                    case WidgetStatus.Approving:
                        WidgetStatus = WidgetStatus.Integrated;
                        break;
                }

                promotionResult.Message = String.Format("Widget {0} was successfully relinquished and its status was reset to {1}.", WidgetId, Status);
            }


            Log4NetHelper.Log(promotionResult.Message, LogLevel.INFO, EntityFormalNamePlural, WidgetId, HttpContext.Current.User.Identity.Name, null);

            return promotionResult;
        }


        private PromotionResult PromoteToIntegrating()
        {
            if (WidgetStatus == WidgetStatus.Created)
            {
                WidgetStatus = WidgetStatus.Integrating;
            }

            PromotionResult promotionResult = new PromotionResult();
            promotionResult.Success = WidgetStatus == WidgetStatus.Integrating;

            if (promotionResult.Success)
                promotionResult.Message = String.Format("Widget {0} successfully claimed by {1} and promoted to status {2}.", WidgetId, HttpContext.Current.User.Identity.Name, WidgetStatus);
            else
                promotionResult.Message = "Failed to promote the widget to Integrating status because its current status prevented it.";

            return promotionResult;
        }


        private PromotionResult PromoteToApproving()
        {
            if (WidgetStatus == WidgetStatus.Integrated)
            {
                WidgetStatus = WidgetStatus.Approving;
            }

            PromotionResult promotionResult = new PromotionResult();
            promotionResult.Success = WidgetStatus == WidgetStatus.Approving;

            if (promotionResult.Success)
                promotionResult.Message = String.Format("Widget {0} successfully claimed by {1} and promoted to status {2}.", WidgetId, HttpContext.Current.User.Identity.Name, WidgetStatus);
            else
                promotionResult.Message = "Failed to promote the widget to Approving status because its current status prevented it.";

            return promotionResult;
        }


        private PromotionResult PromoteToIntegrated()
        {
            PromotionResult promotionResult = new PromotionResult();
            promotionResult.Success = true;

            if (WidgetStatus != WidgetStatus.Integrating)
            {
                promotionResult.Success = false;
                promotionResult.Message = "Failed to promote the widget to Integrated status because its current status prevented it.";
            }

            if (String.IsNullOrWhiteSpace(MainBusCode))
            {
                promotionResult.Success = false;
                promotionResult.Message = "Failed to promote the widget to Integrated status because Main Bus Code was not present.";
            }

            if (promotionResult.Success)
            {
                WidgetStatus = WidgetStatus.Integrated;
                promotionResult.Message = String.Format("Widget {0} successfully promoted to status {1}.", WidgetId, WidgetStatus);
            }

            return promotionResult;
        }


        private PromotionResult PromoteToApproved()
        {
            PromotionResult promotionResult = new PromotionResult();
            promotionResult.Success = true;

            if (WidgetStatus != WidgetStatus.Approving)
            {
                promotionResult.Success = false;
                promotionResult.Message = "Failed to promote the widget to Approved status because its current status prevented it.";
            }

            if (TestPassDateTime == null)
            {
                promotionResult.Success = false;
                promotionResult.Message = "Failed to promote the widget to Approved status because Test Pass Date was not present.";
            }

            if (promotionResult.Success)
            {
                WidgetStatus = WidgetStatus.Approved;
                promotionResult.Message = String.Format("Widget {0} successfully promoted to status {1}.", WidgetId, WidgetStatus);
            }

            return promotionResult;
        }


        private PromotionResult DemoteToCanceled()
        {
            PromotionResult promotionResult = new PromotionResult();
            promotionResult.Success = true;

            if (WidgetStatus != WidgetStatus.Approving && WidgetStatus != WidgetStatus.Integrating)
            {
                promotionResult.Success = false;
                promotionResult.Message = "Failed to demote the widget to Canceled status because its current status prevented it.";
            }

            if (promotionResult.Success)
            {
                WidgetStatus = WidgetStatus.Canceled;
                promotionResult.Message = String.Format("Widget {0} successfully demoted to status {1}.", WidgetId, WidgetStatus);
            }

            return promotionResult;
        }
    }


    public enum WidgetStatus
    {
        Created = 10,
        Integrating = 15,
        Integrated = 20,
        Approving = 25,
        Approved = 30,
        Canceled = -10
    }
}