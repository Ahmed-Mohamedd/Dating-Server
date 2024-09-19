using Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DateContext:DbContext
    {
        public DateContext(DbContextOptions<DateContext> options):base(options) 
        {
                  
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
