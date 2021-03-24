using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectPlutus.Infra.Repositories
{
    public interface IGenericRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> SaveChangesAsync();
    }
}
