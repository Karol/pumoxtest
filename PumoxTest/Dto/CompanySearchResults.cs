using PumoxTest.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTest.Dto
{
    public class CompanySearchResults
    {
        public CompanySearchResults()
        {
            Results = new List<CompanyDto>();
        }

        public List<CompanyDto> Results { get; set; }
    }
}
