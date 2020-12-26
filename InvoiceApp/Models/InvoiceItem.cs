using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Models
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPriceWithoutTax { get; set; }

        public Invoice Invoice { get; set; }
    }
}
