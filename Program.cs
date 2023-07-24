using System;
using System.Net;
using System.Security.Claims;
using System.Text;
using Auth0.AuthenticationApi;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopWebApi.DTOs;
using ShopWebApi.Entities;
using ShopWebApi.Helpers;
using ShopWebApi.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Identity;


namespace ShopWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            
            
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            // Configure AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));
            ConfigureAutoMapper(builder.Services);

            builder.Services.AddAuthentication()
                .AddGoogle("google", opt =>
                {
                    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
                    opt.ClientId = googleAuth["ClientId"];
                    opt.ClientSecret = googleAuth["ClientSecret"];
                    opt.SignInScheme = IdentityConstants.ExternalScheme;
                });

            builder.Services.AddControllers();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shop Api", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop Api V1"); });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Apply database migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                try
                {
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Migration error: {ex.Message}");
                }
            }

            app.Run();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}