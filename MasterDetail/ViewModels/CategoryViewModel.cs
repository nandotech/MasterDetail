using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MasterDetail.Models;

namespace MasterDetail.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }


        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        [Required(ErrorMessage = "You must enter a category name.")]
        [StringLength(20, ErrorMessage = "Category names must be 20 characters or shorter.")]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        public virtual List<InventoryItem> InventoryItems { get; set; }
    }
}