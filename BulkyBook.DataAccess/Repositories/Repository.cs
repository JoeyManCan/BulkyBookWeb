using BulkyBook.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BulkyDbContext _bulkyDbContext;
        protected DbSet<T> _dbSet;

        public Repository(BulkyDbContext bulkyDbContext)
        {
            _bulkyDbContext = bulkyDbContext;
            _dbSet = _bulkyDbContext.Set<T>();
        }
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
         }

        public async Task<T> GetAsync(int id)
        {
            return await _bulkyDbContext.FindAsync<T>(id);
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
