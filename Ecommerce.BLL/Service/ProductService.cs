using Ecommerce.BLL.Extinsions;
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
            // Map the product but SubImages and MainImage will be ignored by Mapster
            var product = request.Adapt<Product>();

            // CRITICAL: Initialize SubImages collection
            product.SubImages = new List<ProductImage>();

            // Handle main image
            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UplodeAsync(request.MainImage);
                if (!string.IsNullOrEmpty(imagePath))
                {
                    product.MainImage = imagePath;
                }
            }

            // Handle sub-images
            if (request.SubImages != null && request.SubImages.Any())
            {
                foreach (var image in request.SubImages)
                {
                    if (image != null && image.Length > 0)
                    {
                        var imagePath = await _fileService.UplodeAsync(image);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            product.SubImages.Add(new ProductImage
                            {
                                ImagePath = imagePath
                            });
                        }
                    }
                }
            }

            await _productRepository.CreateAsync(product);
        }

        public async Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(PaginationRequest request)
        {
            var query =  _productRepository.GetQuaryable(
                null,
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                    "SubImages"
                }
                );
            var pagination = await query.TopaginationAsync(request.Page, request.Limit);
            return new PaginationResponse<ProductResponse>
            {
                Data = pagination.Data.Adapt<List<ProductResponse>>(),
                TotalCount = pagination.TotalCount,
                Page = request.Page,
                Limit = pagination.Limit


            };
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
            var product = await _productRepository.GetOne(c=>c.Id==id,
                new string[] {nameof(Product.SubImages)}
                );
            if (product == null) return false;

             _fileService.Delete(product.MainImage);
            foreach(var item in product.SubImages) {
                _fileService.Delete(item.ImagePath);
            }
            return await _productRepository.DeleteAysnc(product);


        }

        public async Task<bool> UpdateProductAsync(int id, ProductUpdateRequest request)
        {
            var productDb = await _productRepository.GetOne(p => p.Id == id,
                new string[] { nameof(Product.Translations), nameof(Product.SubImages) }
            );
            if (productDb == null) return false;

            var oldImage = productDb.MainImage;

            // Map only non-image properties (SubImages and MainImage are ignored in config)
            request.Adapt(productDb);

            // Ensure SubImages is initialized
            if (productDb.SubImages == null)
            {
                productDb.SubImages = new List<ProductImage>();
            }

            // Handle translations
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

            // Handle main image
            if (request.MainImage != null)
            {
                _fileService.Delete(oldImage);
                productDb.MainImage = await _fileService.UplodeAsync(request.MainImage);
            }

            // Handle sub-images (replace all)
            if (request.SubImages != null)
            {
                foreach (var image in productDb.SubImages)
                {
                    _fileService.Delete(image.ImagePath);
                }
                productDb.SubImages.Clear();

                foreach (var image in request.SubImages)
                {
                    if (image != null && image.Length > 0)
                    {
                        var imagePath = await _fileService.UplodeAsync(image);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            productDb.SubImages.Add(new ProductImage
                            {
                                ImagePath = imagePath
                            });
                        }
                    }
                }
            }

            // Handle new images (add to existing)
            if (request.NewImages != null)
            {
                foreach (var image in request.NewImages)
                {
                    if (image != null && image.Length > 0)
                    {
                        var imagePath = await _fileService.UplodeAsync(image);
                        if (!string.IsNullOrEmpty(imagePath))
                        {
                            productDb.SubImages.Add(new ProductImage { ImagePath = imagePath });
                        }
                    }
                }
            }

            return await _productRepository.UpdateAsync(productDb);
        }
    }
}

