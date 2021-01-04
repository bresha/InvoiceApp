using AutoMapper;
using InvoiceApp.Dtos;
using InvoiceApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;
        private readonly IMapper _mapper;

        public CompanyController(
            ICompanyService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies(string query = null)
        {
            var companies = await _service.GetCompaniesAsync();

            if (!string.IsNullOrWhiteSpace(query))
            {
                companies = companies.Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            var companiesDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companiesDtos);
        }
    }
}
