using Microsoft.EntityFrameworkCore;
using ProjectPlutus.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProjectPlutus.Infra.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ProjectPlutusContext _context;

        public GenericRepository(ProjectPlutusContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public virtual T Add(T entity)
        {
            string[] values = { "CreatedAt", "ModifiedAt" };
            ProcessTimeStamp(entity, values);

            return _context.Add(entity).Entity;
        }

        public virtual T Update(T entity)
        {
            string[] values = { "ModifiedAt" };
            ProcessTimeStamp(entity, values);

            return _context.Update(entity)
                .Entity;
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .AsQueryable()
                .Where(predicate).ToListAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.FindAsync<T>(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsQueryable()
                .ToListAsync();
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        private static void ProcessTimeStamp(T entity, string[] values)
        {
            var currentDateTimeOffset = DateTimeOffset.Now;

            foreach (string value in values)
            {
                typeof(T).GetProperty(value).SetValue(entity, currentDateTimeOffset);
            }
        }
    }
}
