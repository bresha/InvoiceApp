using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        [Required]
        [StringLength(255)]
        public string UserId { get; set; }

        public Company Company { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime PaymentDue { get; set; }

        [Required]
        public byte VATPercentage { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}
