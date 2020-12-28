using InvoiceApp.Models;
using InvoiceApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvoiceApp.UnitTests.TestHelpers
{
    class RegisterData
    {
        public RegisterData()
        {
            CompanyDetails = new CompanyDetails
            {
                Name = "a",
                Address = "a",
                PostalCode = "1",
                City = "a",
                Country = "a",
                VATNumber = "1"
            };

            Model = new RegisterFormViewModel
            {
                FirstName = "a",
                LastName = "a",
                Email = "a@b.c",
                Password = "!Abcd1",
                CompanyDetails = CompanyDetails
            };

            User = new ApplicationUser
            {
                UserName = Model.Email,
                Email = Model.Email,
                FirstName = Model.FirstName,
                LastName = Model.LastName
            };
        }

        public CompanyDetails CompanyDetails { get; set; }
        public RegisterFormViewModel Model { get; set; }
        public ApplicationUser User { get; set; }
    }
}
