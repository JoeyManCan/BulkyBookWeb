using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repositories.IRepositories;

namespace BulkyBook.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; private set; }
        private readonly BulkyDbContext _bulkyDbContext;


        public UnitOfWork(BulkyDbContext bulkyDbContext)
        {
            _bulkyDbContext = bulkyDbContext;
            CategoryRepository = new CategoryRepository(bulkyDbContext);
        }

        protected BulkyDbContext BulkyDbContext
        {
            get { return _bulkyDbContext; }
        }

        public async Task<int> Save()
        {
            return await _bulkyDbContext.SaveChangesAsync();
        }
    }
}
