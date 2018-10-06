using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTest.Dto
{ 
    public class EmployeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
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
