using BulkyBook.DataAccess.Repositories.IRepositories;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BulkyDbContext bulkyDbContext) : base(bulkyDbContext)
        {

        }

        public Product UpdateProductValues(Product dbProduct, Product productParam)
        {
            dbProduct.Title = productParam.Title;
            dbProduct.ISBN = productParam.ISBN;
            dbProduct.Price = productParam.Price;
            dbProduct.Price50 = productParam.Price50;
            dbProduct.ListPrice = productParam.ListPrice;
            dbProduct.Price100 = productParam.Price100;
            dbProduct.Description = productParam.Description;
            dbProduct.CategoryId = productParam.CategoryId;
            dbProduct.Author = productParam.Author;
            dbProduct.CoverTypeId = productParam.CoverTypeId;

            //if there's no new image, the image would be gone
            //without a null check
            if (productParam.ImageUrl != null)
            {
                dbProduct.ImageUrl = productParam.ImageUrl;
            }

            return dbProduct;
        }

        public async Task<EntityState> Update(Product productParam)
        {
            var dbProduct = await Context.Products.FirstOrDefaultAsync(
                prod => prod.Id == productParam.Id);

            //restricting update
            if (dbProduct != null)
            {
                dbProduct = UpdateProductValues(dbProduct, productParam);

                return Context.Update(dbProduct).State;
            }

            return EntityState.Unchanged;

        }
    }
}
