using PumoxTest.DataBase.Entities;
using PumoxTest.DataBase.Repositories;

namespace PumoxTest.DataBase
{
    public interface IUnitOfWork
    {
        GenericRepository<Company> CompanyRepository { get; }
        GenericRepository<Employe> EmployeRepository { get; }

        void Dispose();
        void Save();
    }
}