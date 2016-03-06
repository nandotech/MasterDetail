using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MasterDetail.DataLayer;

namespace MasterDetail.Models
{
    public static class WorkListBusinessRules
    {
        private static ApplicationDbContext _applicationDbContext = new ApplicationDbContext();


        public static PromotionResult CanClaimWorkListItem(string userId)
        {
            const int MaximumNumberOfWorkListItemsAUserMayClaim = 3;
            int numberOfClaimedWorkListItems = 0;

            numberOfClaimedWorkListItems += _applicationDbContext.WorkOrders.Count(wo => wo.CurrentWorkerId == userId);
            //numberOfClaimedWorkListItems += _applicationDbContext.Widgets.Count(wi => wi.CurrentWorkerId == userId);

            PromotionResult promotionResult = new PromotionResult { Success = true };

            if (numberOfClaimedWorkListItems >= MaximumNumberOfWorkListItemsAUserMayClaim)
            {
                promotionResult.Message = String.Format("You cannot claim any more work list items because you already have {0} and the maximum number allowed is {1}", numberOfClaimedWorkListItems, MaximumNumberOfWorkListItemsAUserMayClaim);
                promotionResult.Success = false;
            }

            return promotionResult;
        }
    }
}