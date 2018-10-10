using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PumoxTest.Dto;
using PumoxTest.Services;

namespace PumoxTest.Controllers
{
    [Authorize]
    [ApiController]
    public class CompanyController : Controller
    {
        private ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Gets a company by id with all employees
        /// </summary>
        /// <param name="id">Company Id</param>
        [ProducesResponseType(typeof(CompanyDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("company/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                CompanyDto companyDto = _companyService.Get(id);
                if (companyDto == null)
                {
                    return NotFound($"No company with id: {id}");
                }
                return Ok(companyDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.ToString());
            }
        }

        /// <summary>
        /// Gets all company with all employees
        /// </summary>
        /// <returns>List<CompanyDto></returns>
        [ProducesResponseType(typeof(List<CompanyDto>), 200)]
        [ProducesResponseType(500)]
        [Route("company")]
        public IActionResult Get()
        {
            try
            {
                return Ok(_companyService.GetAll());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("company/create")]
        public IActionResult Create([FromBody] CompanyDto body)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ValidateDto validate = _companyService.Validate(body);
                if (!validate.IsValid)
                {
                    return BadRequest(validate.Msg);
                }

                long id = _companyService.Create(body);

                return StatusCode(201, new { id });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(CompanyDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("company/update/{id}")]
        public IActionResult Update(long id, [FromBody] CompanyDto body)
        {
            try
            {
                var companyDto = _companyService.Get(id);

                if (companyDto == null)
                {
                    return NotFound($"No company with id: {id}");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _companyService.Update(id, body);

                return Ok(body);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("company/delete/{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                var companyDto = _companyService.Get(id);

                if (companyDto == null)
                {
                    return NotFound($"No company with id: {id}");
                }

                _companyService.Delete(companyDto);

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
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("company/search")]
        public IActionResult Search([FromBody] CompanySearchRequest body)
        {
            try
            {
                if (body == null)
                {
                    return BadRequest();
                }

                CompanySearchResults result = _companyService.Search(body);
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}