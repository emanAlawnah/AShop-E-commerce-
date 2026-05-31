using Ecommerce.DAL.Data;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.PL.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDataBaseService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>

            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
        

    }
}
