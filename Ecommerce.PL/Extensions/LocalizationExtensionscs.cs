using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Ecommerce.PL.Extensions
{
    public static class LocalizationExtensionscs
    {
        public static IServiceCollection AddLocalizationServise (this IServiceCollection services,IConfiguration configuration)
        {

           services.AddLocalization(options => options.ResourcesPath = "");
            const string defaultCulture = "en";
            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture),
                new CultureInfo("ar")
            };
            services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();

                //options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
                //{
                //    QueryStringKey = "lang"
                //});

                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });

            return services;
        }
    }
}
