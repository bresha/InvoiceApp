using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.CustomValidation
{
    public class EnsurePaymentDueIsAtLeast8DaysLater : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var date = value as DateTime?;
            if (date != null)
            {
                return date > DateTime.Now.AddDays(8);
            }

            return false;
        }
    }
}
