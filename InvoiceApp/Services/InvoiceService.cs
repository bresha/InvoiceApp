using InvoiceApp.Helpers;
using InvoiceApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateInvoiceAsync(Invoice invoice, ApplicationUser user)
        {
            var company = await GetCompanyAsync(invoice.Company);

            if (company == null)
            {
                var result = await CheckDoesCompanyNameAlreadyExistAsync(invoice.Company);
                if (result.Count > 0)
                {
                    return $"There is already company with name '{invoice.Company.Name}' but properties '{string.Join(", ", result)}' are different.";
                }

                result = await CheckDoesCompanyVATNumberAlreadyExistAsync(invoice.Company);
                if (result.Count > 0)
                {
                    return $"There is already company with VAT number '{invoice.Company.VATNumber}' but properties '{string.Join(", ", result)}' are different.";

                }

                invoice.UserId = user.Id;
                invoice.CreatedAt = DateTime.Now;

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                return "";
            }

            invoice.Company = company;
            invoice.UserId = user.Id;
            invoice.CreatedAt = DateTime.Now;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return "";
        }

        private async Task<List<string>> CheckDoesCompanyNameAlreadyExistAsync(Company company)
        {
            var output = new List<string>();
            var companyByName = await _context.Companies.SingleOrDefaultAsync(c => c.Name == company.Name);

            if (companyByName != null)
            {
                output = ObjectChecker.PublicInstancePropertiesEqual<Company>(company, companyByName, "CompanyId", "Invoices");
            }

            return output;
        }

        private async Task<List<string>> CheckDoesCompanyVATNumberAlreadyExistAsync(Company company)
        {
            var output = new List<string>();
            var companyByVATNumber = await _context.Companies.SingleOrDefaultAsync(c => c.VATNumber == company.VATNumber);

            if (companyByVATNumber != null)
            {
                output = ObjectChecker.PublicInstancePropertiesEqual<Company>(company, companyByVATNumber, "CompanyId", "Invoices");
            }

            return output;
        }

        private async Task<Company> GetCompanyAsync(Company company)
        {
            var output = await _context.Companies.SingleOrDefaultAsync(
                c => c.Name == company.Name && 
                c.VATNumber == company.VATNumber &&
                c.Address == company.Address &&
                c.PostalCode == company.PostalCode &&
                c.City == company.City &&
                c.Country == company.Country &&
                c.Email == company.Email &&
                c.Phone == company.Phone);

            return output;
        }
    }
}
