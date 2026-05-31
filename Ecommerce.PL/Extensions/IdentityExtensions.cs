using Ecommerce.DAL.Data;
using Ecommerce.DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecommerce.PL.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,IConfiguration configuration) 
        
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.User.RequireUniqueEmail = true;
                Options.Password.RequireDigit = true;
                Options.Password.RequireLowercase = true;
                Options.Password.RequireUppercase = true;
                Options.Password.RequireNonAlphanumeric = true;
                Options.Password.RequiredLength = 5;

                Options.Lockout.MaxFailedAccessAttempts = 5;
                Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

            }


               )
               .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

           
            return services;
        }
    }
}
