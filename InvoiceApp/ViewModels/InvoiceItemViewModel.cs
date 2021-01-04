using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.ViewModels
{
    public class InvoiceItemViewModel
    {
        [Required]
        [StringLength(5000, MinimumLength = 3)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be greater than {1}.")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Unit price without tax")]
        [Range(0.01, (double) decimal.MaxValue, ErrorMessage = "Value for {0} must be greater than {1}.")]
        public decimal UnitPriceWithoutTax { get; set; }
    }
}
