using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasterDetail.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }

        [Required(ErrorMessage = "You must enter an Item Code.")]
        [StringLength(15, ErrorMessage = "The Item Code must be 15 characters or shorter.")]
        [Display(Name = "Item Code")]
        public string InventoryItemCode { get; set; }

        [Required(ErrorMessage = "You must enter a Name.")]
        [StringLength(80, ErrorMessage = "The Name must be 80 characters or shorter.")]
        [Display(Name = "Name")]
        public string InventoryItemName { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
    }
}