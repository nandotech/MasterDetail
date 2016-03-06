using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterDetail.Models
{
    public class PromotionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }


        public PromotionResult()
        {
            Success = false;
        }
    }
}