﻿using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess
{
    public class BulkyDbContext : DbContext
    {
        public BulkyDbContext(DbContextOptions<BulkyDbContext> options):base(options)
        {

        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<CoverType> CoverTypes { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

    }
}
