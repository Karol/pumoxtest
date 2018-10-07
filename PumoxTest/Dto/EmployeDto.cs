using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTest.Dto
{
    public class EmployeDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EnumDataType(typeof(JobTitle), ErrorMessage = "Given JobTitle is invalid. Available JobTitle are: Administrator, Developer, Architect, Manager")]
        public string JobTitle { get; set; }
    }

    public enum JobTitle
    {
        Administrator,
        Developer,
        Architect,
        Manager
    }
}
