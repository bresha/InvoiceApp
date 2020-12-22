using InvoiceApp.Controllers;
using InvoiceApp.Models;
using InvoiceApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace InvoiceApp.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly RegisterData _registerData;

        public AccountControllerTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _registerData = new RegisterData();
        }

        [Fact]
        public async Task Register_ThereIsNoAdminInDatabase_ReturnsDefaultView()
        {
            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s.CheckIsAnyAdmin()).ReturnsAsync(false);
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Register();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Register_ThereIsAnAdminInDatabase_ReturnsRedirectToLogin()
        {
            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s.CheckIsAnyAdmin()).ReturnsAsync(true);
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Register();
            Assert.IsType<RedirectToActionResult>(result);

            var resultOfRedirect = (RedirectToActionResult)result;
            Assert.Equal("Login", resultOfRedirect.ActionName);
            Assert.Equal("Account", resultOfRedirect.ControllerName);
        }

        [Fact]
        public async Task Register_ControllerSignsInUser_ReturnsRedirectToIndexOfHomeController()
        {
            List<string> list = new List<string>();
            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s
            .AddFirstUserWithCompanyDetails(It.IsAny<ApplicationUser>(), It.IsAny<RegisterFormViewModel>()))
                .ReturnsAsync(list);
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Register(_registerData.Model);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task Register_ModedStateIsInvalid_ReturnsView()
        {
            List<string> list = new List<string> { "a", "b" };
            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s
            .AddFirstUserWithCompanyDetails(It.IsAny<ApplicationUser>(), It.IsAny<RegisterFormViewModel>()))
                .ReturnsAsync(list);
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Register(_registerData.Model);

            var viewResult = Assert.IsType<ViewResult>(result);
            var numberOfErrors = viewResult.ViewData.ModelState.ErrorCount;
            Assert.Equal(2, numberOfErrors);
        }

        private Mock<SignInManager<ApplicationUser>> GetMockSignInManager()
        {
            var userStoreMock = new Mock<IUserRoleStore<ApplicationUser>>();
            var mockUsrMgr = new UserManager<ApplicationUser>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var ctxAccessor = new HttpContextAccessor();
            var mockClaimsPrinFact = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var mockOpts = new Mock<IOptions<IdentityOptions>>();
            var mockLogger = new Mock<ILogger<SignInManager<ApplicationUser>>>();
            var mockAuthSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
            var mockUserConf = new Mock<IUserConfirmation<ApplicationUser>>();

            return new Mock<SignInManager<ApplicationUser>>(
                mockUsrMgr, 
                ctxAccessor, 
                mockClaimsPrinFact.Object, 
                mockOpts.Object, 
                mockLogger.Object, 
                mockAuthSchemeProvider.Object, 
                mockUserConf.Object);
        }
    }
}
