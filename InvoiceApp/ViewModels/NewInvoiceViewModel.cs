using InvoiceApp.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.ViewModels
{
    public class NewInvoiceViewModel
    {

        public CompanyViewModel Company { get; set; }

        [Display(Name = "Payment Due")]
        public DateTime PaymentDue { get; set; }

        [Required]
        [Display(Name = "VAT Percentage")]
        public byte VATPercentage { get; set; }

        [EnsureOneInvoiceItem(ErrorMessage = "At least one invoice item is required.")]
        public List<InvoiceItemViewModel> InvoiceItems { get; set; }
    }
}
