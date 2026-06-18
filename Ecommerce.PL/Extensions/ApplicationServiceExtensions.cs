using Ecommerce.BLL.Service;
using Ecommerce.DAL.Repository;
using Ecommerce.DAL.Utiles;
using Stripe;

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
            services.AddScoped<IFileService, BLL.Service.FileService>();
            services.AddScoped<IProductService, BLL.Service.ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICheckoutService, BLL.Service.CheckoutService>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            // Configure Stripe settings
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
             StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];



            return services;
        }
    }
}
