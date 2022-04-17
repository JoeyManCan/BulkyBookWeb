using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(int id);
        EntityState Delete(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
