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
            //Context.Products.Include(product => product.Category).Include(product => product.CoverType);
        }
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public EntityState Delete(T entity)
        {
            return _dbSet.Remove(entity).State;
        }

        private IQueryable<T> SeparateIncludeProperties(string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (includeProperties != null)
            {
                //Separates each property in the string separated by ","
                foreach(var property in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    //the Include method here is used to associated entities (Category, CoverType) to the top entity (Product)
                    query = query.Include(property);
                }
            }

            return query;
        }

        //includeProperties - "Category, CoverType"
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            var query = SeparateIncludeProperties(includeProperties);

            return query.ToList();
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

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            var query = SeparateIncludeProperties(includeProperties);
            return query.FirstOrDefault(filter)!;
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

    }
}
