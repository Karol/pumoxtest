using AutoMapper;
using PumoxTest.DataBase;
using PumoxTest.DataBase.Entities;
using PumoxTest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PumoxTest.Services
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetAll();
        CompanyDto Get(long id);
        ValidateDto Validate(CompanyDto companyDto);
        long Create(CompanyDto companyDto);
        CompanySearchResults Search(CompanySearchRequest searchRequest);
        void Update(long id, CompanyDto companyDto);
        void Delete(CompanyDto companyDto);
    }

    public class CompanyService : BaseService, ICompanyService
    {
        public CompanyService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public ValidateDto Validate(CompanyDto companyDto)
        {
            var companyDb = _unitOfWork.CompanyRepository.GetFirstOrDefault(filter: x => x.Name.Equals(companyDto.Name));
            if (companyDb != null)
            {
                return new ValidateDto() { IsValid = false, Msg = $"Company: '{companyDto.Name}' already exists in the database" };
            }
            return new ValidateDto();            
        }

        public long Create(CompanyDto companyDto)
        {
            var companyDb = _mapper.Map<Company>(companyDto);
            _unitOfWork.CompanyRepository.Insert(companyDb);
            _unitOfWork.Save();
            return companyDb.Id;
        }

        public void Delete(CompanyDto companyDto)
        {
            Company company = _mapper.Map<Company>(companyDto);
            _unitOfWork.CompanyRepository.Delete(company);
            _unitOfWork.Save();
        }

        public CompanyDto Get(long id)
        {
            var companyDto = _mapper.Map<CompanyDto>(_unitOfWork.CompanyRepository.GetFirstOrDefaultInclude(filter: x => x.Id == id, includeProperties: x => x.Employees));
            return companyDto;
        }

        public IEnumerable<CompanyDto> GetAll()
        {
            var companyDbList = _unitOfWork.CompanyRepository.Get(includeProperties: x=>x.Employees);
            List<CompanyDto> companyDtoList = _mapper.Map<List<CompanyDto>>(companyDbList);
            return companyDtoList;
        }

        public CompanySearchResults Search(CompanySearchRequest searchRequest)
        {
            var companyDbList = _unitOfWork.CompanyRepository.Get(x => x.Name.Contains(searchRequest.Keyword) || x.Employees.Any(y =>
                  y.FirstName.Contains(searchRequest.Keyword)
               || y.LastName.Contains(searchRequest.Keyword)
               || y.DateOfBirth <= searchRequest.EmployeeDateOfBirthFrom
               || y.DateOfBirth >= searchRequest.EmployeeDateOfBirthFrom
               || searchRequest.EmployeeJobTitles.Contains(y.JobTitle))
               , includeProperties: x => x.Employees).ToList();

            return new CompanySearchResults
            {
                Results = _mapper.Map<List<CompanyDto>>(companyDbList)
            };

        }

        public void Update(long id, CompanyDto companyDto)
        {
            Company company = _unitOfWork.CompanyRepository.GetFirstOrDefaultInclude(filter: x => x.Id == id, includeProperties: x => x.Employees);
            company.Name = companyDto.Name;
            company.EstablishmentYear = companyDto.EstablishmentYear;
            company.Employees = _mapper.Map<List<Employe>>(companyDto.Employees);
            _unitOfWork.CompanyRepository.Update(company);
            _unitOfWork.Save();
        }
    }
}
