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
        private readonly BulkyDbContext _bulkyDbContext;
        public ICategoryRepository CategoryRepository { get; private set; }
        public ICoverTypeRepository CoverTypeRepository { get; private set; }


        public UnitOfWork(BulkyDbContext bulkyDbContext)
        {
            _bulkyDbContext = bulkyDbContext;
            CategoryRepository = new CategoryRepository(bulkyDbContext);
            CoverTypeRepository = new CoverTypeRepository(bulkyDbContext);
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
