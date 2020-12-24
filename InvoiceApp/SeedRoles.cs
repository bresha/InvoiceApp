using InvoiceApp.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp
{
    public static class SeedRoles
    {
        public static async Task CreateAdminRoleAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var alreadyExists = await roleManager.RoleExistsAsync(Roles.Admin);

            if (alreadyExists) return;

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }
    }
}
