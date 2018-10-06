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
                Company company = UnitOfWork.CompanyRepository.GetInclude(x=>x.Id==id, "Employees").FirstOrDefault();
                if (company == null)
                {
                    return NotFound($"No company with id: {id}");
                }
                return Ok(company);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpPost]
        [Route("company/create")]
        public IActionResult Create([FromBody] Company company)
        {
            try
            {
                UnitOfWork.CompanyRepository.Insert(company);
                UnitOfWork.Save();

                return Ok(new { company.Id });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Company), 200)]
        [Route("company/update/{id}")]
        public IActionResult Update(long id, [FromBody] Company body)
        {
            try
            {
                Company company = UnitOfWork.CompanyRepository.GetInclude(x => x.Id == id, "Employees").First();
                company.Name = body.Name;
                company.EstablishmentYear = body.EstablishmentYear;
                company.Employees = body.Employees;
                UnitOfWork.CompanyRepository.Update(company);
                UnitOfWork.Save();

                return Ok(company);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpDelete]
        [Route("company/delete/{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                Company company = UnitOfWork.CompanyRepository.GetInclude(x => x.Id == id, "Employees").First();
                UnitOfWork.CompanyRepository.Delete(company);
                UnitOfWork.Save();

                return Ok("Delete sukcess");
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