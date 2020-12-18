using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class UserCompanyDetails
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Street Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(255)]
        public string City { get; set; }

        [Required]
        [StringLength(255)]
        public string Country { get; set; }

        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Company Phone")]
        public string Phone { get; set; }

        [StringLength(255)]
        [EmailAddress]
        [Display(Name = "Company Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "VAT Number")]
        public string VATNumber { get; set; }
    }
}
