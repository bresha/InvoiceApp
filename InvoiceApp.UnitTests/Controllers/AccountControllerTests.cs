using InvoiceApp.Controllers;
using InvoiceApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace InvoiceApp.UnitTests.Controllers
{
    public class AccountControllerTests : IDisposable
    {
        private readonly AppServiceCollection _service;
        private readonly ITestOutputHelper _outputHelper;

        public AccountControllerTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _service = new AppServiceCollection();
        }

        public void Dispose()
        {
            _service.Destroy(_service);
        }

        [Fact]
        public async Task Register_ThereIsNoAdminInDatabase_ReturnsDefaultView()
        {
            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s.CheckIsAnyAdmin()).ReturnsAsync(false);
            var controller = new AccountController(_service.SignInManager, mockService.Object);

            var result = await controller.Register();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Register_ThereIsAnAdminInDatabase_ReturnsRedirectToLogin()
        {
            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s.CheckIsAnyAdmin()).ReturnsAsync(true);
            var controller = new AccountController(_service.SignInManager, mockService.Object);

            var result = await controller.Register();
            Assert.IsType<RedirectToActionResult>(result);

            var resultOfRedirect = (RedirectToActionResult)result;
            Assert.Equal("Login", resultOfRedirect.ActionName);
            Assert.Equal("Account", resultOfRedirect.ControllerName);
        }
    }
}
