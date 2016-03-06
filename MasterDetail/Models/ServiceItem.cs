using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasterDetail.Models
{
    public class ServiceItem
    {
        public int ServiceItemId { get; set; }
       
        [Required(ErrorMessage = "You must enter an Item Code.")]
        [StringLength(15, ErrorMessage = "The Item Code must be 15 characters or shorter.")]
        [Display(Name = "Item Code")]
        public string ServiceItemCode { get; set; }

        [Required(ErrorMessage = "You must enter a Name.")]
        [StringLength(80, ErrorMessage = "The Name must be 80 characters or shorter.")]
        [Display(Name = "Name")]
        public string ServiceItemName { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Rate { get; set; }
    }
}