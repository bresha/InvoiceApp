using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class Company
    {
        public int CompanyId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(255)]
        public string City { get; set; }

        [Required]
        [StringLength(255)]
        public string Country { get; set; }

        [Required]
        [StringLength(50)]
        public string VATNumber { get; set; }

        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        public List<Invoice> Invoices { get; set; }
    }
}
