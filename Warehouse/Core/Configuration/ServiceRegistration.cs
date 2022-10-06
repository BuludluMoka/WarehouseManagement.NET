using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Warehouse.Core.Validator.Products;
using Warehouse.Data.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Warehouse.Data.Models.Common.Authentication;
using Warehouse.Core.Validator.CustomValidation;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Warehouse.Data.Models.Common.Jwt;

namespace Warehouse.Core.Configuration
{
    public static class ServiceRegistration
    {

        public static void AddServices(this IServiceCollection services)
        {

            /*.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve)*/



           
            services.AddEndpointsApiExplorer();
            services.AddCors();


            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Warehouse Api", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });
            services.AddDbContext<WarehouseDbContext>(options => options.UseSqlServer(ConfigurationDb.ConnectionString));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Identity Basic Config
            services.AddIdentity<AppUser, AppRole>(_ =>
            {
                _.Password.RequiredLength = 6;
                _.Password.RequireNonAlphanumeric = false;
                _.Password.RequireDigit = false;
                _.Password.RequireLowercase = false;
                _.Password.RequireUppercase = false;

                _.User.RequireUniqueEmail = true;
                _.User.AllowedUserNameCharacters = "abcçdefghiıjklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._@+";
            })
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<WarehouseDbContext>()
                .AddDefaultTokenProviders();




            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "http://karfree-001-site1.atempurl.com",
                    ValidIssuer = "http://karfree-001-site1.atempurl.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"))
                };
            });
        }

    }
}
