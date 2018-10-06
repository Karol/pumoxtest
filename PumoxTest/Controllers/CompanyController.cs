using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PumoxTest.DataBase.Entities;

namespace PumoxTest.Controllers
{
    [Authorize]
    [ApiController]
    public class CompanyController : BaseController
    {
        public CompanyController(IConfiguration config) : base(config)
        {
        }

        [ProducesResponseType(typeof(Company), 200)]
        [Route("company/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var company = UnitOfWork.CompanyRepository.GetInclude(x=>x.Id==id, "Employees");
                return Ok(company);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}