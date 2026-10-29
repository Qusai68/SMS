using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SMS.Models;

namespace SMS.DbContexts
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);// Call the Parent  method
                                               // Seeding Database ---
            modelBuilder.Entity<Lookup>().HasData(
                // Employee Posigints (MajorCode=0)
                //new Lookup { Id = 1, MajorCode = 0, MinorCode = 0, Name = "Class Grade" },
                //new Lookup { Id = 2, MajorCode = 0, MinorCode = 1, Name = "1" },
                //new Lookup { Id = 3, MajorCode = 0, MinorCode = 2, Name = "2" },
                //new Lookup { Id = 4, MajorCode = 0, MinorCode = 3, Name = "3" },
                //new Lookup { Id = 5, MajorCode = 0, MinorCode = 4, Name = "4" },
                //new Lookup { Id = 6, MajorCode = 0, MinorCode = 5, Name = "5" },
                //new Lookup { Id = 7, MajorCode = 0, MinorCode = 6, Name = "6" },
                //new Lookup { Id = 8, MajorCode = 0, MinorCode = 7, Name = "7" },
                //new Lookup { Id = 9, MajorCode = 0, MinorCode = 8, Name = "8" },
                //new Lookup { Id = 10, MajorCode = 0, MinorCode = 9, Name = "9" },
                //new Lookup { Id = 11, MajorCode = 0, MinorCode = 10, Name = "10" },
                //new Lookup { Id = 12, MajorCode = 0, MinorCode = 11, Name = "11" },
                //new Lookup { Id = 13, MajorCode = 0, MinorCode = 12, Name = "12" },
                //// Departmernt Types (MajorCode=1)
                //new Lookup { Id = 5, MajorCode = 1, MinorCode = 0, Name = "Class Section" },
                //new Lookup { Id = 6, MajorCode = 1, MinorCode = 1, Name = "A" },
                //new Lookup { Id = 7, MajorCode = 1, MinorCode = 2, Name = "B" },
                //new Lookup { Id = 8, MajorCode = 1, MinorCode = 3, Name = "C" },
                //new Lookup { Id = 9, MajorCode = 1, MinorCode = 4, Name = "D" }


                );
           
        }

        // Table Creation
        public DbSet<Student> Student { get; set; }
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Lookup> Lookup { get; set; }
        public DbSet<User> Users { get; set; }
    }

    
}
