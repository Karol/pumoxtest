using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using PumoxTest.Controllers;
using PumoxTest.DataBase;
using PumoxTest.DataBase.Entities;
using System;
using Xunit;

namespace PumoxTest.UnitTests
{
    public class CompanyControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IConfiguration> _configuration;
        private Mock<IMapper> _mapper;

        [Fact]
        public void Get()
        {
            // Arrange
            _unitOfWork = new Mock<IUnitOfWork>();
            _configuration = new Mock<IConfiguration>();
            _mapper = new Mock<IMapper>();
            //_unitOfWork.Setup(u => u.CompanyRepository.GetFirstOrDefaultInclude(filter: x => x.Id == test, includeProperties: txt)).Returns((Company)null);

            var controller = new CompanyController(_configuration.Object, _mapper.Object, _unitOfWork.Object);

            // Act
            var result = controller.Get(123);

            // Assert
            var notFoundObjectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal("No company with id: 123", notFoundObjectResult.Value);
        }
    }
}
