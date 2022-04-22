using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<EntityState> Update(Product product);
        Product UpdateProductValues(Product dbProduct, Product productParam);
    }
}
