using Microsoft.AspNetCore.Mvc.Rendering;
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
        IEnumerable<T> GetAll(string? includeProperties = null);
        Task Add(T entity);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task<T> GetAsync(int id);
        EntityState Delete(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
