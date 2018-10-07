using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        public CompanyController(IConfiguration config, IMapper mapper) : base(config, mapper)
        {
        }

        [ProducesResponseType(typeof(CompanyDto), 200)]
        [Route("company/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                CompanyDto company = Mapper.Map<CompanyDto>(UnitOfWork.CompanyRepository.GetInclude(x=>x.Id==id, "Employees").FirstOrDefault());
                if (company == null)
                {
                    return NotFound($"No company with id: {id}");
                }

                CompanyDto companyDto = Mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [ProducesResponseType(typeof(CompanyDto), 200)]
        [Route("company")]
        public IActionResult Get()
        {
            try
            {
                var allDbCompany = UnitOfWork.CompanyRepository.GetInclude("Employees");

                List<CompanyDto> companyDto = Mapper.Map<List<CompanyDto>>(allDbCompany);
                return Ok(companyDto);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpPost]
        [Route("company/create")]
        public IActionResult Create([FromBody] CompanyDto company)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return BadRequest(message);
                }
                var companyDb = Mapper.Map<Company>(company);
                UnitOfWork.CompanyRepository.Insert(companyDb);
                UnitOfWork.Save();

                return Ok(new { companyDb.Id });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(CompanyDto), 200)]
        [Route("company/update/{id}")]
        public IActionResult Update(long id, [FromBody] CompanyDto body)
        {
            try
            {
                Company company = UnitOfWork.CompanyRepository.GetInclude(x => x.Id == id, "Employees").FirstOrDefault();
                if (company == null)
                {
                    return NotFound($"No company with id: {id}");
                }


                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return BadRequest(message);
                }

                company.Name = body.Name;
                company.EstablishmentYear = body.EstablishmentYear;
                company.Employees = Mapper.Map<List<Employe>>(body.Employees);
                UnitOfWork.CompanyRepository.Update(company);
                UnitOfWork.Save();

                CompanyDto companyDto = Mapper.Map<CompanyDto>(company);
                return Ok(companyDto);
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

                Company company = UnitOfWork.CompanyRepository.GetInclude(x => x.Id == id, "Employees").FirstOrDefault();

                if (company == null)
                {
                    return NotFound($"No company with id: {id}");
                }

                UnitOfWork.CompanyRepository.Delete(company);
                UnitOfWork.Save();

                return Ok("Delete success");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
        
        [AllowAnonymous]
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

                var companyDbList = UnitOfWork.CompanyRepository.GetInclude(x => x.Name.Contains(request.Keyword) || x.Employees.Any(y => 
                   y.FirstName.Contains(request.Keyword) 
                || y.LastName.Contains(request.Keyword) 
                || y.DateOfBirth <= request.EmployeeDateOfBirthFrom
                || y.DateOfBirth >= request.EmployeeDateOfBirthFrom
                || request.EmployeeJobTitles.Contains(y.JobTitle))
                , "Employees").ToList();

                CompanySearchResults result = new CompanySearchResults();
                result.Results = Mapper.Map<List<CompanyDto>>(companyDbList);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}