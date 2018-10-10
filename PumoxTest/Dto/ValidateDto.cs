using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTest.Dto
{
    public class ValidateDto
    {
        public bool IsValid { get; set; }
        public string Msg { get; set; }

        public ValidateDto()
        {
            IsValid = true;
        }
    }
}
