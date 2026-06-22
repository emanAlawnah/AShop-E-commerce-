using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public interface IProductService
    {
        Task CreateProduct(ProductRequest request);
        Task<PaginationResponse<ProductResponse>> GetAllProductsAsync(PaginationRequest request);
        Task<ProductResponse> GetProduct(Expression<Func<Product, bool>> filter);
        Task<bool> DeleteProduct(int id);

        Task<bool> UpdateProductAsync(int id, ProductUpdateRequest request);
    }
}
