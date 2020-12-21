using InvoiceApp.Constants;
using InvoiceApp.Models;
using InvoiceApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceApp.UnitTests
{
    public class AppServiceCollection
    {
        public AppServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection
                .AddSingleton<IConfiguration>(new ConfigurationBuilder().Build())
                .AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Test");
                    options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                })
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            serviceCollection.AddLogging();

            Provider = serviceCollection.BuildServiceProvider();

            Context = Provider.GetRequiredService<ApplicationDbContext>();

            UserManager = Provider.GetRequiredService<UserManager<ApplicationUser>>();

            SignInManager = Provider.GetRequiredService<SignInManager<ApplicationUser>>();

            RoleManager = Provider.GetRequiredService<RoleManager<IdentityRole>>();

            // Add admin role to db as it is seeded in application
            RoleManager.CreateAsync(new IdentityRole(Roles.Admin));
        }

        public ServiceProvider Provider { get; }
        public ApplicationDbContext Context { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; set; }

        public ILogger<T> GetLogger<T>()
        {
            return Provider.GetRequiredService<ILogger<T>>();
        }

        public void Destroy(AppServiceCollection service)
        {
            service.Context.Database.EnsureDeleted();
            service.Context.Dispose();
        }
    }
}
