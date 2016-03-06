using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasterDetail.Models
{
    public class Part
    {
        public int PartId { get; set; }
        public int WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; }

        [Required(ErrorMessage = "You must enter an Item Code.")]
        [StringLength(15, ErrorMessage = "The Item Code must be 15 characters or shorter.")]
        [Display(Name = "Item Code")]
        public string InventoryItemCode { get; set; }

        [Required(ErrorMessage = "You must enter a Name.")]
        [StringLength(80, ErrorMessage = "The Name must be 80 characters or shorter.")]
        [Display(Name = "Name")]
        public string InventoryItemName { get; set; }

        [Required(ErrorMessage = "You must enter a Quantity.")]
        [Range(1, 1000000, ErrorMessage = "The Quantity must be between 1 and 1,000,000.")]
        public int Quantity { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Extended")]
        public decimal ExtendedPrice { get; set; }

        [StringLength(140, ErrorMessage = "Notes must be 140 characters or shorter.")]
        public string Notes { get; set; }

        [Display(Name = "Installed")]
        public bool IsInstalled { get; set; }
    }
}