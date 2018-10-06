using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PumoxTest.DataBase.Entities;
using PumoxTest.Dto;

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
                Company company = UnitOfWork.CompanyRepository.GetInclude(x=>x.Id==id, "Employees").First();
                return Ok(company);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CompanySearchResults), 200)]
        [Route("company/search")]
        public IActionResult Search([FromBody] CompanySearchRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest();
                }

                CompanySearchResults result = new CompanySearchResults();
                result.Results = UnitOfWork.CompanyRepository.GetInclude(x => x.Name.Contains(request.Keyword) || x.Employees.Any(y => 
                   y.FirstName.Contains(request.Keyword) 
                || y.LastName.Contains(request.Keyword) 
                || y.DateOfBirth <= request.EmployeeDateOfBirthFrom
                || y.DateOfBirth >= request.EmployeeDateOfBirthFrom
                || request.EmployeeJobTitles.Contains(y.JobTitle))
                , "Employees").ToList();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}