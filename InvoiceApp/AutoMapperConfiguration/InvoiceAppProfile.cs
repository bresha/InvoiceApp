using AutoMapper;
using InvoiceApp.Models;
using InvoiceApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.AutoMapperConfiguration
{
    public class InvoiceAppProfile : Profile
    {
        public InvoiceAppProfile()
        {
            CreateMap<NewInvoiceViewModel, Invoice>();
            CreateMap<CompanyViewModel, Company>();
            CreateMap<InvoiceItemViewModel, InvoiceItem>();
        }
    }
}
