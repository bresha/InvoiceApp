using InvoiceApp.Controllers;
using InvoiceApp.Models;
using InvoiceApp.Services;
using InvoiceApp.UnitTests.TestHelpers;
using InvoiceApp.ViewModels;
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
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace InvoiceApp.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly RegisterData _registerData;
        private readonly LoginViewModel _loginData;

        public AccountControllerTests()
        {
            _registerData = new RegisterData();
            _loginData = new LoginViewModel { Email = "a.b@c", Password = "!Abcd1", RememberMe = false };
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

            var resultOfRedirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", resultOfRedirect.ActionName);
            Assert.Equal("Account", resultOfRedirect.ControllerName);
        }

        [Fact]
        public async Task Register_ControllerSignsInUser_ReturnsRedirectToIndexOfHomeController()
        {
            List<string> list = new List<string>();
            var mockService = new Mock<IAccountService>();
            mockService.Setup(
                s => s.AddFirstUserWithCompanyDetails(It.IsAny<ApplicationUser>(), It.IsAny<RegisterFormViewModel>()))
                .ReturnsAsync(list);
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Register(_registerData.Model);

            mockSignInManager.Verify(m => m.SignInAsync(It.IsAny<ApplicationUser>(), false, It.IsAny<string>()), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public async Task Register_ModedStateIsInvalid_ReturnsViewWithErorsInModelState()
        {
            List<string> list = new List<string> { "a", "b" };
            var mockService = new Mock<IAccountService>();
            mockService.Setup(
                s => s.AddFirstUserWithCompanyDetails(It.IsAny<ApplicationUser>(), It.IsAny<RegisterFormViewModel>()))
                .ReturnsAsync(list);
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Register(_registerData.Model);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(2, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsType<RegisterFormViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Login_ThereIsAnAdmin_ReturnsDefaultView()
        {
            var mockService = new Mock<IAccountService>();
            mockService.Setup(s => s.CheckIsAnyAdmin()).ReturnsAsync(true);
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Login();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Login_ThereIsNoAdmin_RedirectsToRegister()
        {
            var mockService = new Mock<IAccountService>();
            mockService.Setup(m => m.CheckIsAnyAdmin()).ReturnsAsync(false);
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Login();

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Register", redirectResult.ActionName);
            Assert.Equal("Account", redirectResult.ControllerName);
        }

        [Fact]
        public async Task Login_SignInUserSuccessful_RedirectToIndexOfHomeController()
        {
            var mockService = new Mock<IAccountService>();
            var mockSignInManager = GetMockSignInManager();
            mockSignInManager.Setup(
                m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Login(_loginData);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public async Task Login_SignInUserUnsuccessful_ReturnsDefaultViewWithModelAndAnErrorInModelState()
        {
            var mockService = new Mock<IAccountService>();
            var mockSignInManager = GetMockSignInManager();
            mockSignInManager.Setup(
                m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Login(_loginData);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsType<LoginViewModel>(viewResult.ViewData.Model);                 
        }

        [Fact]
        public async Task Logout_IsCalled_SignsOutUserAndRedirectsToLogin()
        {
            var mockService = new Mock<IAccountService>();
            var mockSignInManager = GetMockSignInManager();
            var controller = new AccountController(mockSignInManager.Object, mockService.Object);

            var result = await controller.Logout();

            mockSignInManager.Verify(m => m.SignOutAsync(), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
            Assert.Equal("Account", redirectResult.ControllerName);
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
