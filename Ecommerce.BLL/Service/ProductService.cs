using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using Ecommerce.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public class ProductService :IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository,IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }

        public async Task CreateProduct(ProductRequest request)
        {
            var product= request.Adapt<Product>();

            if(request.MainImage != null)
            {
                var imagePath = await _fileService.UplodeAsync(request.MainImage);
                product.MainImage = imagePath;
            }
            await _productRepository.CreateAsync(product);

        }

        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync(
                null,
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                }
                );
            return products.Adapt<List<ProductResponse>>();
        }

        public async Task <ProductResponse> GetProduct(Expression<Func<Product,bool>> filter)
        {
            var product= await _productRepository.GetOne(filter,
                 new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                }
                );
            if (product == null) return null;
            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOne(c=>c.Id==id);
            if (product == null) return false;

             _fileService.Delete(product.MainImage);
            return await _productRepository.DeleteAysnc(product);


        }

        public async Task<bool> UpdateProductAsync(int id, ProductUpdateRequest request)
        {
            var productDb = await _productRepository.GetOne(p => p.Id == id,
                new string[] { nameof(Product.Translations) }
                );
            if (productDb == null) return false;

            var oldImage = productDb.MainImage;

            if (request.Translations != null)
            {
                foreach (var translationresponse in request.Translations)
                {
                    var existing = productDb.Translations.FirstOrDefault(t => t.Language == translationresponse.Language);
                    if (existing != null)
                    {
                        if (translationresponse.Name != null)
                        {
                            existing.Name = translationresponse.Name;
                        }
                        if (translationresponse.Description != null)
                        {
                            existing.Description = translationresponse.Description;
                        }

                    }
                    else
                    {
                        return false;
                    }


                }
            }

                request.Adapt(productDb);


            if (request.MainImage != null)
            {
                _fileService.Delete(oldImage);

                productDb.MainImage =
                    await _fileService.UplodeAsync(request.MainImage);
            }

            return await _productRepository.UpdateAsync(productDb);
        }
    }
}

