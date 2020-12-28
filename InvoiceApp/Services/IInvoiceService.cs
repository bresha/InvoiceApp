using InvoiceApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Services
{
    public interface IInvoiceService
    {
        Task<string> CreateInvoiceAsync(Invoice invoice, ApplicationUser user);
    }
}
