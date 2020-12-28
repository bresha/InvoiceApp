using AutoMapper;
using InvoiceApp.Models;
using InvoiceApp.Services;
using InvoiceApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(
            ILogger<InvoiceController> logger, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IInvoiceService invoiceService)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> New(NewInvoiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null) return Challenge();

                var invoice = _mapper.Map<Invoice>(model);

                var error = await _invoiceService.CreateInvoiceAsync(invoice, currentUser);

                if (string.IsNullOrWhiteSpace(error))
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", error);
            }

            return View(model);
        }
    }
}
