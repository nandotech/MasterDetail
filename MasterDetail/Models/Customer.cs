using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasterDetail.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Display(Name = "Account #")]
        [Required(ErrorMessage = "You must enter an account number.")]
        [StringLength(8, ErrorMessage = "The account number must be 8 characters or shorter.")]
        public string AccountNumber { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "You must enter a company name.")]
        [StringLength(30, ErrorMessage = "The company name must be 30 characters or shorter.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "You must enter an address.")]
        [StringLength(30, ErrorMessage = "The address must be 30 characters or shorter.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "You must enter a city.")]
        [StringLength(15, ErrorMessage = "The city must be 15 characters or shorter.")]
        public string City { get; set; }

        [Required(ErrorMessage = "You must enter a state.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "The state must be exactly 2 characters long.")]
        public string State { get; set; }

        [Display(Name = "ZIP Code")]
        [Required(ErrorMessage = "You must enter a ZIP Code.")]
        [StringLength(10, ErrorMessage = "The ZIP Code must be 10 characters or shorter.")]
        public string ZipCode { get; set; }

        [StringLength(15, ErrorMessage = "The phone number must be 15 characters or shorter.")]
        public string Phone { get; set; }

        public List<WorkOrder> WorkOrders { get; set; }

        public bool Cloaked { get; set; }
    }
}