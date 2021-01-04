﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Dtos
{
    public class CompanyDto
    {
        public int CompanyId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string VATNumber { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
