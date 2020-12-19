using InvoiceApp.Constants;
using InvoiceApp.Models;
using InvoiceApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            IAccountService accountService)
        {
            _signInManager = signInManager;
            _accountService = accountService;
        }

        public IActionResult Register()
        {
            // TODO: Add logic for disabling registration where is an admin
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _accountService.AddFirstUserWithCompanyDetails(user, model);

                if (!result.Any())
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(model);
        }
    }
}
