using ProjectPlutus.Data.Context;
using ProjectPlutus.Domain.Models;
using ProjectPlutus.Infra.Repositories;
using System;
using System.Threading.Tasks;

namespace ProjectPlutus.Infra
{
    public interface IUnitOfWork
    {
        IGenericRepository<Employee> EmployeeRepository { get; }
        Task<bool> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private ProjectPlutusContext _context;

        public UnitOfWork(ProjectPlutusContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }


        private IGenericRepository<Employee> employeeRepository;
        public IGenericRepository<Employee> EmployeeRepository
        {
            get
            {
                if (employeeRepository == null)
                {
                    employeeRepository = new EmployeeRepository(_context);
                }

                return employeeRepository;
            }
        }


        public virtual async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
