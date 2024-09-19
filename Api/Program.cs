
using Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DateContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            var app = builder.Build();

            #region Update Database

            // Group Of servives Which it's lifetime is scope
            using var Scope = app.Services.CreateScope();

            // Services ItSelf
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                //Ask CLR To Create Object From ApplicationDbContext Explicitly
                var DateContext = Services.GetRequiredService<DateContext>();
                //var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
                //var UserManager = services.GetRequiredService<UserManager<User>>();

                await DateContext.Database.MigrateAsync();
                //await IdentityDbContext.Database.MigrateAsync();

                //await ApplicationContextSeed.SeedAsync(DbContext, LoggerFactory);
                //await AppIdentityDbContextSeeding.SeedUserAsync(UserManager);

            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occurred While Updating Database");
            }

            #endregion

            #region  Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            #endregion
        }
    }
}
