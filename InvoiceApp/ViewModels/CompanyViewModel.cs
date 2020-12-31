using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.ViewModels
{
    public class CompanyViewModel
    {
        [Required]
        [StringLength(255, MinimumLength = 3)]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        [Display(Name = "Company Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string City { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Country { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "VAT Number")]
        public string VATNumber { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [StringLength(255, MinimumLength = 3)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
