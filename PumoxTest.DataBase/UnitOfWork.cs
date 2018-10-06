using PumoxTest.DataBase.Repositories;
using System;

namespace PumoxTest.DataBase
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed;
        private PumoxTestContext Context { get; set; }

        public UnitOfWork(PumoxTestContext dbContext)
        {
            disposed = false;
            Context = dbContext;
        }

        public UnitOfWork(string connectionString) : this(PumoxTestContext.Create(connectionString))
        {
        }

        private GenericRepository<Entities.Company> _companyRepository { get; set; }
        public GenericRepository<Entities.Company> CompanyRepository
        {
            get
            {
                if (this._companyRepository == null)
                {
                    this._companyRepository = new GenericRepository<Entities.Company>(Context);
                }
                return _companyRepository;
            }
        }

        private GenericRepository<Entities.Employe> _employeRepository { get; set; }
        public GenericRepository<Entities.Employe> EmployeRepository
        {
            get
            {
                if (this._employeRepository == null)
                {
                    this._employeRepository = new GenericRepository<Entities.Employe>(Context);
                }
                return _employeRepository;
            }
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
