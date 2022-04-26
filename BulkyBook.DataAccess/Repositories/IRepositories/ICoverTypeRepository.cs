using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.IRepositories
{
    public interface ICoverTypeRepository:IRepository<CoverType>
    {
        EntityState Update(CoverType coverType);
        public IEnumerable<SelectListItem> ReturnSelectListItems();
    }
}
