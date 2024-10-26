
using Api.Data;
using Api.Data.Seeding;
using Api.Extensions;
using Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // allow di for context
            builder.Services.AddDbContext<DateContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddAuthenticationServices(builder.Configuration);

            //allow cors for client app
            builder.Services.AddCors();
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

                await UserSeeding.SeedAsync(DateContext, LoggerFactory);
                //await AppIdentityDbContextSeeding.SeedUserAsync(UserManager);

            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occurred While Updating Database");
            }

            #endregion


            #region  Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
             {
                    app.UseSwagger();
                    app.UseSwaggerUI();
             }
                app.UseRouting();
                app.UseStaticFiles();
                app.UseHttpsRedirection();  // convert any protocol to https protocol for more security

                app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200")); 
            
                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

            #endregion

             app.Run();
        }
    }
}
