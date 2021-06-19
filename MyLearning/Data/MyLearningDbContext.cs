using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLearning.Models;

    public class MyLearningDbContext : DbContext
    {
        public MyLearningDbContext (DbContextOptions<MyLearningDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
    }
