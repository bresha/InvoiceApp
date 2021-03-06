﻿using InvoiceApp.Constants;
using InvoiceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                            results.Add(error.Description);
                    }
                    else
                    {
                        var resultOfAddingAdminRole = await _userManager.AddToRoleAsync(user, Roles.Admin);

                        if (!resultOfAddingAdminRole.Succeeded)
                        {
                            foreach (var error in resultOfAddingAdminRole.Errors)
                                results.Add(error.Description);
                        }
                        else
                        {
                            _context.CompanyDetails.Add(model.CompanyDetails);

                            await _context.SaveChangesAsync();

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    results.Add("Failed to add admin with company detils.");

                    _logger.LogError(ex, "Add first user with company details to database failed.");

                    await transaction.RollbackAsync();
                }
            }

            return results;
        }

        public async Task<bool> CheckIsAnyAdmin()
        {
            return await (from users in _context.Users
                          join userroles in _context.UserRoles on users.Id equals userroles.UserId
                          join roles in _context.Roles on userroles.RoleId equals roles.Id
                          where roles.Name == Roles.Admin
                          select users).AnyAsync();

        }
    }
}
