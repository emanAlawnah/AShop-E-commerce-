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
    public class CategorySarvice : ICategorySarvice
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategorySarvice(ICategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }
        public async Task <CategoryResponse> CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
           await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }

        public async Task <List<CategoryResponse>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync(
            new string[] {nameof(Category.Translations),
            nameof(Category.CreatedBy)
            
            });
            return categories.Adapt<List<CategoryResponse>>();
        }

        public async Task<CategoryResponse> GetCategory(Expression<Func<Category, bool>> filter)
        {
            var category = await _categoryRepository.GetOne(filter,new string[] {nameof(Category.Translations)});   
            return category.Adapt<CategoryResponse>();
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetOne(c=>c.Id==id);
            if (category == null) return false;
            else return await _categoryRepository.DeleteAysnc(category);
        }
    }
}
