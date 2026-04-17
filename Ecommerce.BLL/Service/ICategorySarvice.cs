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
    public interface ICategorySarvice
    {
       Task <List<CategoryResponse>> GetAllCategories();
        Task <CategoryResponse> CreateCategory(CategoryRequest request);
        Task<CategoryResponse> GetCategory(Expression<Func<Category, bool>> filter);
        Task<bool> DeleteCategory(int id);
    }
}
