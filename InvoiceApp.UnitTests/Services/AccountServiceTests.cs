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
        private readonly RegisterFormViewModel _model;
        private readonly ApplicationUser _user;

        public AccountServiceTests()
        {
            _service = new AppServiceCollection();

            var registerData = new RegisterData();
            _model = registerData.Model;
            _user = registerData.User;
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
