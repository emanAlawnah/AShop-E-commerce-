namespace Ecommerce.PL.Extensions
{
    public static  class AddCorsPolicy
    {
        public const string policeName = "_myAllowSpecificOrigins";
        public static IServiceCollection AddCorsPolicySrrvice (this IServiceCollection services,IConfiguration configuration)
        {


            services.AddCors(options =>
            {
                options.AddPolicy(name: policeName,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

                                  });
            });

            return services;
        }
    }
}
