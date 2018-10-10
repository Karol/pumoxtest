using Microsoft.AspNetCore.Mvc;
using Moq;
using PumoxTest.Controllers;
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
                .Returns(GetTestCompanees().First());

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Get(companyId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var companyDto = Assert.IsType<CompanyDto>(okResult.Value);
            Assert.Equal("test", companyDto.Name);
            Assert.Equal(2018, companyDto.EstablishmentYear);
        }
       
        [Fact]
        public void GetCompany_Create_StatusCode_201()
        {
            
            // Arrange
            var mockService = new Mock<ICompanyService>();
            long id = 12;

            var newCompanyDto = new CompanyDto()
            {
                Name = "test new",
                EstablishmentYear = 2018,
                Employees = new List<EmployeDto>() { new EmployeDto() { LastName = "Jan", FirstName = "Nowak", DateOfBirth = DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
            };
            mockService.Setup(s => s.Validate(newCompanyDto))
                .Returns(new ValidateDto());

            mockService.Setup(s => s.Create(newCompanyDto))
                .Returns(id);

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Create(newCompanyDto);

            var okResult = Assert.IsType<ObjectResult>(result);            
            Assert.Equal(201, okResult.StatusCode);
            mockService.Verify();           
        }

        [Fact]
        public void GetCompany_Create_CompanyWithNameExist()
        {

            // Arrange
            var mockService = new Mock<ICompanyService>();
            var newCompanyDto = new CompanyDto()
            {
                Name = "test",
                EstablishmentYear = 2018,
                Employees = new List<EmployeDto>() { new EmployeDto() { LastName = "Jan", FirstName = "Nowak", DateOfBirth = DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
            };

            mockService.Setup(s => s.Validate(newCompanyDto))
                .Returns(new ValidateDto() { IsValid = false, Msg= $"Company: '{newCompanyDto.Name}' already exists in the database" });

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Create(newCompanyDto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal($"Company: '{newCompanyDto.Name}' already exists in the database" , badRequest.Value);
            mockService.Verify();
        }


        [Fact]
        public void GetCompany_Create_BadRequest()
        {
            // Arrange
            var mockService = new Mock<ICompanyService>();

            var newCompanyDto = new CompanyDto()
            {
                Name = "test",
                EstablishmentYear = 2018,
                Employees = new List<EmployeDto>() { new EmployeDto() { LastName = "Jan", FirstName = "Nowak", DateOfBirth = DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
            };

            var controller = new CompanyController(mockService.Object);
          
            // Assert
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = controller.Create(newCompanyDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetCompany_Update_Ok()
        {

            // Arrange
            long companyId = 12;
            var mockService = new Mock<ICompanyService>();

            var updateCompanyDto = new CompanyDto()
            {
                Name = "newCompany",
                EstablishmentYear = 1236,
                Employees = new List<EmployeDto>() { new EmployeDto() { LastName = "Jan", FirstName = "Nowak", DateOfBirth = DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
            };

            mockService.Setup(s => s.Get(companyId))
                .Returns(GetTestCompanees().First());

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Update(companyId ,updateCompanyDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var companyDto = Assert.IsType<CompanyDto>(okResult.Value);
            Assert.Equal("newCompany", companyDto.Name);
            Assert.Equal(1236, companyDto.EstablishmentYear);
            mockService.Verify();
        }

        [Fact]
        public void GetCompany_Update_NotFound()
        {
            // Arrange
            long companyId = 12;
            var mockService = new Mock<ICompanyService>();

            var updateCompanyDto = new CompanyDto()
            {
                Name = "12344",
                EstablishmentYear = 1236,
                Employees = new List<EmployeDto>() { new EmployeDto() { LastName = "Jan", FirstName = "Nowak", DateOfBirth = DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
            };

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Update(companyId, updateCompanyDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetCompany_Update_BadRequest()
        {
            // Arrange
            long companyId = 12;
            var mockService = new Mock<ICompanyService>();

            var updateCompanyDto = new CompanyDto()
            {
                Name = "",
                Employees = new List<EmployeDto>() { new EmployeDto() { LastName = "Jan", FirstName = "Nowak", DateOfBirth = DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
            };

            mockService.Setup(s => s.Get(companyId))
                .Returns(GetTestCompanees().First());

            var controller = new CompanyController(mockService.Object);

            // Assert
            controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = controller.Update(companyId, updateCompanyDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetCompany_Delete_Ok()
        {
            // Arrange
            long companyId = 12;
            var mockService = new Mock<ICompanyService>();

            mockService.Setup(s => s.Get(companyId))
                .Returns(GetTestCompanees().First());

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Delete(companyId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetCompany_Delete_NotFound()
        {
            // Arrange
            long companyId = 12;
            var mockService = new Mock<ICompanyService>();

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Delete(companyId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetCompany_Search_Ok()
        {
            // Arrange
            var mockService = new Mock<ICompanyService>();
            var seaech = new CompanySearchRequest() { Keyword = "test" };
            mockService.Setup(s => s.Search(seaech))
                .Returns(new CompanySearchResults() { Results = GetTestCompanees() });

            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Search(seaech);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var companySearchResults = Assert.IsType<CompanySearchResults>(okResult.Value);
            Assert.NotEmpty(companySearchResults.Results);
            Assert.Equal(2, companySearchResults.Results.Count);
        }

        [Fact]
        public void GetCompany_Search_BadRequest()
        {
            // Arrange
            var mockService = new Mock<ICompanyService>();
            var controller = new CompanyController(mockService.Object);

            // Act
            var result = controller.Search(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        private List<CompanyDto> GetTestCompanees()
        {
            var companees = new List<CompanyDto>();
            companees.Add(new CompanyDto()
            {
                Name = "test",
                EstablishmentYear = 2018,
                Employees = new List<EmployeDto>() { new EmployeDto() { LastName = "Jan", FirstName = "Nowak", DateOfBirth = DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
            });
            companees.Add(new CompanyDto()
            {
                Name = "test 2",
                EstablishmentYear = 1987,
                Employees = new List<EmployeDto>() { new EmployeDto() { LastName = "Adam", FirstName = "Nowak", DateOfBirth = DateTime.Now, JobTitle = JobTitle.Developer.ToString() } }
            });
            return companees;
        }
    }
}
