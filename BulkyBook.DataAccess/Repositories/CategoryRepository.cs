using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public CategoryRepository(BulkyDbContext bulkyDbContext):base(bulkyDbContext)
        {
        }

        public EntityState Update(Category category)
        {
            return Context.Update(category).State;
        }

        public IEnumerable<SelectListItem> ReturnSelectListItems()
        {
            var result = ReturnSelectListItems<Category, SelectListItem>(
                cat => new SelectListItem()
            {
                Text = cat.Name,
                Value = cat.Id.ToString()
            });

            return result;
        }
    }
}
