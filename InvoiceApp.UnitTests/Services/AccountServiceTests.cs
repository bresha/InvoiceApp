using InvoiceApp.Constants;
using InvoiceApp.Models;
using InvoiceApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace InvoiceApp.UnitTests.Services
{
    public class AccountServiceTests : IDisposable
    {
        private readonly AppServiceCollection _service;
        private readonly CompanyDetails _companyDetails;
        private readonly RegisterFormViewModel _model;
        private readonly ApplicationUser _user;
        private readonly ITestOutputHelper _outputHelper;

        public AccountServiceTests(ITestOutputHelper outputHelper)
        {
            _service = new AppServiceCollection();

            _companyDetails = new CompanyDetails
            {
                Name = "a",
                Address = "a",
                PostalCode = "1",
                City = "a",
                Country = "a",
                VATNumber = "1"
            };

            _model = new RegisterFormViewModel
            {
                FirstName = "a",
                LastName = "a",
                Email = "a@b.c",
                Password = "!Abcd1",
                CompanyDetails = _companyDetails
            };

            _user = new ApplicationUser
            {
                UserName = _model.Email,
                Email = _model.Email,
                FirstName = _model.FirstName,
                LastName = _model.LastName
            };
            _outputHelper = outputHelper;
        }

        public void Dispose()
        {
            _service.Destroy(_service);
        }

        [Fact]
        public async Task CheckIsAnyAdmin_ThereIsNoAdmins_ReturnFalse()
        {
            var service = new AccountService(_service.Context, _service.UserManager, _service.GetLogger<AccountService>());

            var result = await service.CheckIsAnyAdmin();

            Assert.False(result);
        }

        [Fact]
        public async Task CheckIsAnyAdmin_ThereAreAnAdmin_ReturnTrue()
        {
            await _service.UserManager.CreateAsync(_user, _model.Password);
            await _service.UserManager.AddToRoleAsync(_user, Roles.Admin);
            var service = new AccountService(_service.Context, _service.UserManager, _service.GetLogger<AccountService>());

            var result = await service.CheckIsAnyAdmin();

            Assert.True(result);           
        }

        [Fact]
        public async Task AddFirstUserWithCompanyDetails_AddsUserWithCompanyDetails_ReturnsEmptyList()
        {
            var service = new AccountService(_service.Context, _service.UserManager, _service.GetLogger<AccountService>());

            var result = await service.AddFirstUserWithCompanyDetails(_user, _model);

            Assert.Empty(result);
        }

        [Fact]
        public async Task AddFirstUserWithCompanyDetails_DoesntAddUserWithCompanyDetails_ReturnsListOfErrors()
        {
            var service = new AccountService(_service.Context, _service.UserManager, _service.GetLogger<AccountService>());
            _model.CompanyDetails = null;

            var result = await service.AddFirstUserWithCompanyDetails(_user, _model);

            Assert.NotEmpty(result);
            Assert.Collection(result, item => Assert.Equal("Failed to add admin with company detils.", item));
        }
    }
}
