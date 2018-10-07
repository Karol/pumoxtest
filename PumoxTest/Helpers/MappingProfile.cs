using AutoMapper;
using PumoxTest.DataBase.Entities;
using PumoxTest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTest.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<Employe, EmployeDto>();

            CreateMap<CompanyDto, Company>().ForMember(desc => desc.Id, opt=>opt.Ignore());
            CreateMap<EmployeDto, Employe>().ForMember(desc => desc.Id, opt => opt.Ignore());
        }           
    }
}
