using Api.Data.Entities;
using Api.Data.TypesConventions;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DateContext:DbContext
    {
        public DateContext(DbContextOptions<DateContext> options):base(options) 
        {
                  
        }

        public DbSet<AppUser> Users { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            configurationBuilder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>();
        }
    }
}
