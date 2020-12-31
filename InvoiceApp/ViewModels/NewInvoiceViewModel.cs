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
        [DataType(DataType.Date)]
        [EnsurePaymentDueIsAtLeast8DaysLater(ErrorMessage = "{0} must be at least 8 days after today.")]
        public DateTime PaymentDue { get; set; }

        [Required]
        [Display(Name = "VAT Percentage")]
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public byte VATPercentage { get; set; }

        [EnsureOneInvoiceItem(ErrorMessage = "At least one invoice item is required.")]
        public List<InvoiceItemViewModel> InvoiceItems { get; set; }
    }
}
