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
<<<<<<< HEAD
=======

        

>>>>>>> 58b4ee77e0fe94b2fff59c5dac536358bd791fe5
     
    }
}