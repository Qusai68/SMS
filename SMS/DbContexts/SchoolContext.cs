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

            // Table Creation
          public DbSet<Student> Students { get; set; }
        }

    
}
