using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.Category_Id, source => source.Id)
                .Map(dest => dest.UserCreated, source => source.CreatedBy.UserName)
                .Map(dest=>dest.Name,source=>source.Translations.Where
                (
                    t=>t.Language== CultureInfo.CurrentCulture.Name).Select(t=>t.Name).FirstOrDefault()
                
                );

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
     .Map(dest => dest.UserCreated,
         source => source.CreatedBy.UserName)

     .Map(dest => dest.Name,
         source => source.Translations
         .Where(t => t.Language ==
             CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
         .Select(t => t.Name)
         .FirstOrDefault())

     .Map(dest => dest.MainImage,
         source => $"https://localhost:7232/images/{source.MainImage}")

     .Map(dest => dest.SubImages,
         source => source.SubImages
         .Select(i => $"https://localhost:7232/images/{i.ImagePath}"));

            TypeAdapterConfig<OrderItem, OrderItemResponse>.NewConfig()
                       .Map(dest => dest.ProductName, src => src.Product.Translations.Where(t => t.Language ==
                    CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                .Select(t => t.Name)
                .FirstOrDefault());

     TypeAdapterConfig<Cart, CartResponse>.NewConfig()
    .Map(dest => dest.ProductName,
        source => source.Product.Translations
        .Where(t => t.Language ==
            CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
        .Select(t => t.Name)
        .FirstOrDefault())
    .Map(dest => dest.Price,
        source => source.Product.Price)
    .Map(dest => dest.ProductImage,
        source => $"https://localhost:7232/images/{source.Product.MainImage}");


            TypeAdapterConfig<ProductRequest, Product>.NewConfig()
    .Ignore(dest => dest.SubImages)
    .Ignore(dest => dest.MainImage).
    IgnoreNullValues(true);

            TypeAdapterConfig<ProductUpdateRequest, Product>.NewConfig()
                .Ignore(dest => dest.SubImages)
                .Ignore(dest => dest.MainImage)
                .IgnoreNullValues(true);


        }
    }
}
