using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace cSharp_LibrarySystemWebAPI
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddControllers();
            // to connect to the DB
            builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

            // Cors service
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            //EF
            //builder.Services.AddDbContext<LibraryDbContext>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //JWT 
            builder.Services.AddAuthentication(options => { options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Mohammed",
                    ValidAudience = "TRA",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"))

                };
            });
            //Services.AddDistributedMemoryCache(); // Use an appropriate distributed cache in a production environment.
            //services.AddSession()
            //SeriLog from old way
            //Log.Logger = new LoggerConfiguration()
            //                 .MinimumLevel.Information()
            //                 .WriteTo.File("D:\\Log\\Logs.txt", rollingInterval: RollingInterval.Hour)
            //                 .CreateLogger();

            //Logging configs from Appsettings.json
            Log.Logger = new LoggerConfiguration()
                             .ReadFrom.Configuration(builder.Configuration)
                             .CreateLogger();

            builder.Host.UseSerilog();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Serilog request logging
            app.UseSerilogRequestLogging();
            // Cors middleware
            app.UseCors("AllowAll");

            app.UseAuthentication(); //JWT
            app.UseAuthorization();

            

            

            app.MapControllers();

            app.Run();
        }
    }
}