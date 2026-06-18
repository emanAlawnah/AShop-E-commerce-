
using Ecommerce.BLL;
using Ecommerce.BLL.Mapping;
using Ecommerce.BLL.Service;
using Ecommerce.DAL.Data;
using Ecommerce.DAL.Models;
using Ecommerce.DAL.Repository;
using Ecommerce.DAL.Utiles;
using Ecommerce.PL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                 });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();




             


            builder.Services.AddDataBaseService(builder.Configuration);
            builder.Services.AddIdentityService(builder.Configuration);
            builder.Services.AddAuthenetcationServise(builder.Configuration);
            builder.Services.AddLocalizationServise(builder.Configuration);

            builder.Services.AddApplicationServices(builder.Configuration);



            builder.Services.AddCorsPolicySrrvice(builder.Configuration);






            MapsterConfig.MapsterConfigRegister();
      

            var app = builder.Build();
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);




            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseCors(AddCorsPolicy.policeName);

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
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
