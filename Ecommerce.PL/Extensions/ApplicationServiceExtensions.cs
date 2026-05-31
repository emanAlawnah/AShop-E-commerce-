using Ecommerce.BLL.Service;
using Ecommerce.DAL.Repository;
using Ecommerce.DAL.Utiles;

namespace Ecommerce.PL.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration) 
        
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategorySarvice, CategorySarvice>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();



            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<ISeedData, RoleSeedData>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();


            return services;
        }
    }
}
