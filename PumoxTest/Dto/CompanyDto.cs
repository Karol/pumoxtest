using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTest.Dto
{
    public class CompanyDto
    {
        public CompanyDto()
        {
            Employees = new List<EmployeDto>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public int EstablishmentYear { get; set; }

        public ICollection<EmployeDto> Employees { get; set; }
    }
}
