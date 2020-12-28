using InvoiceApp.Models;
using InvoiceApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Services
{
    public interface IAccountService
    {
        Task<List<string>> AddFirstUserWithCompanyDetails(ApplicationUser user, RegisterFormViewModel model);
        Task<bool> CheckIsAnyAdmin();
    }
}
