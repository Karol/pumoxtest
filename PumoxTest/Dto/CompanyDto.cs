using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public int EstablishmentYear { get; set; }
        public ICollection<EmployeDto> Employees { get; set; }
    }
}
