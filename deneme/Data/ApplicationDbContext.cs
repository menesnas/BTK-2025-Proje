using deneme.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace deneme.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet tanımlamaları
        public DbSet<Product> Products { get; set; }

        

     
    }
}