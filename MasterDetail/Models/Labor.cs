using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasterDetail.Models
{
    public class Labor
    {
        public int LaborId { get; set; }
        public int WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; }

        [Required(ErrorMessage = "You must enter an Item Code.")]
        [StringLength(15, ErrorMessage = "The Item Code must be 15 characters or shorter.")]
        [Display(Name = "Item Code")]
        public string ServiceItemCode { get; set; }

        [Required(ErrorMessage = "You must enter a Name.")]
        [StringLength(80, ErrorMessage = "The Name must be 80 characters or shorter.")]
        [Display(Name = "Name")]
        public string ServiceItemName { get; set; }

        [Required(ErrorMessage = "You must enter the number of Labor Hours.")]
        [Range(1, 100000, ErrorMessage = "The Labor Hours must be between 1 and 100,000.")]
        [Display(Name = "Labor Hours")]
        public decimal LaborHours { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Rate { get; set; }

        [Display(Name = "Extended")]
        public decimal ExtendedPrice { get; set; }

        [StringLength(140, ErrorMessage = "Notes must be 140 characters or shorter.")]
        public string Notes { get; set; }

        [Display(Name = "% Complete")]
        [Range(0, 100, ErrorMessage = "Percent Complete must be between zero and 100.")]
        public int PercentComplete { get; set; }
    }
}