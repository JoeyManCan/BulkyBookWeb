using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly BulkyDbContext _bulkyDbContext;
        public CategoryRepository(BulkyDbContext bulkyDbContext):base(bulkyDbContext)
        {
        }
        public async Task<int> SaveChanges()
        {
            return await _bulkyDbContext.SaveChangesAsync();
        }

        public void Update(Category category)
        {
            _bulkyDbContext.Update(category);
        }
    }
}
