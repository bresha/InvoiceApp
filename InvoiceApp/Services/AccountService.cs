using InvoiceApp.Constants;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            ILogger<AccountService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<List<string>> AddFirstUserWithCompanyDetails(ApplicationUser user, RegisterFormViewModel model)
        {
            var results = new List<string>();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var resultOfCreatingUser = await _userManager.CreateAsync(user, model.Password);

                    if (!resultOfCreatingUser.Succeeded)
                    {
                        foreach (var error in resultOfCreatingUser.Errors)
                        {
                            results.Add(error.Description);
                        }
                    }
                    else
                    {
                        var resultOfAddingAdminRole = await _userManager.AddToRoleAsync(user, Roles.Admin);

                        if (!resultOfAddingAdminRole.Succeeded)
                        {
                            foreach (var error in resultOfAddingAdminRole.Errors)
                            {
                                results.Add(error.Description);
                            }
                        }
                        else
                        {
                            _context.CompanyDetails.Add(model.CompanyDetails);
                            
                            var resultOfAddingCompanyDetails = await _context.SaveChangesAsync();

                            if (resultOfAddingCompanyDetails != 1)
                            {
                                results.Add("Failed to add company details.");
                            }
                            else
                            {
                                transaction.Commit();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Add first user with company details to database failed.");
                    await transaction.RollbackAsync();
                }
            }

            return results;
        }
    }
}
