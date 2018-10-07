using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using PumoxTest.Controllers;
using PumoxTest.DataBase;
using PumoxTest.DataBase.Entities;
using PumoxTest.Dto;
using PumoxTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PumoxTest.UnitTests
{
    public class CompanyControllerTests
    {
        [Fact]
        public void GetCompany_ById_NoIdInDb()
        {
            int companyId =12;
            // Arrange
            var mockService = new Mock<ICompanyService>();
            mockService.Setup(s => s.Get(companyId))
                .Returns((CompanyDto)null);

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Get(companyId);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"No company with id: {companyId}", notFoundObjectResult.Value);
        }

        [Fact]
        public void GetCompany_ById_IdInDb()
        {
            int companyId = 12;
            // Arrange
            var mockService = new Mock<ICompanyService>();
            mockService.Setup(s => s.Get(companyId))
                .Returns(new CompanyDto
                {
                    Name = "test",
                    EstablishmentYear = 2018,
                    Employees = new List<EmployeDto>() { new EmployeDto() {LastName="Adam", FirstName="Nowak",DateOfBirth=DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
                });

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Get(companyId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var companyDto = Assert.IsType<CompanyDto>(okResult.Value);
            Assert.Equal("test", companyDto.Name);
        }
    }
}
