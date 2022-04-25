using BulkyBook.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> _dbSet;
        protected BulkyDbContext Context;

        public Repository(BulkyDbContext bulkyDbContext)
        {
            _dbSet = bulkyDbContext.Set<T>();
            Context = bulkyDbContext;
        }
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public EntityState Delete(T entity)
        {
            return _dbSet.Remove(entity).State;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        protected IEnumerable<TResult> ReturnSelectListItems<TSource, TResult>
            (Func<T, TResult> lambdaFunction) 
        {
            var result = GetAll().Result.Select(lambdaFunction);

            return result;
        }


        public async Task<T> GetAsync(int id)
        {

            return await Context.FindAsync<T>(id);

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
