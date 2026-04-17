
using Ecommerce.BLL;
using Ecommerce.DAL.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Ecommerce.DAL.Repository;
using Ecommerce.BLL.Service;
using Ecommerce.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Ecommerce.DAL.Utiles;
using System.Threading.Tasks;

namespace Ecommerce.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>

            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddLocalization(options => options.ResourcesPath = "");
            const string defaultCulture = "en";
            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture),
                new CultureInfo("ar")
            };
            builder.Services.Configure<RequestLocalizationOptions>(options => {
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

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategorySarvice,CategorySarvice>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options => Options.User.RequireUniqueEmail= true)
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddScoped<ISeedData,RoleSeedData>();
            var app = builder.Build();
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope()) 
            {
            var services= scope.ServiceProvider;
            var seeders= services.GetServices<ISeedData>();
                foreach(var seeder in seeders)
                {
                    await seeder.DataSeed();
                }
            }
            app.Run();
        }
    }
}
