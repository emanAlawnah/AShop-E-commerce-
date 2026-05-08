using Ecommerce.BLL.Service;
using Ecommerce.DAL.Data;
using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using Ecommerce.DAL.Repository;
using Ecommerce.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace Ecommerce.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
       private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategorySarvice _categorySarvice;
      public  CategoriesController(ICategorySarvice categorySarvice,IStringLocalizer<SharedResources> localizer) {
       
        _localizer = localizer;
        _categorySarvice = categorySarvice;
        }

        
        [HttpPost("")]
        [Authorize]
        public async Task <IActionResult> Create(CategoryRequest request)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _categorySarvice.CreateCategory(request);
            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
           
        }

            
            
        [HttpGet("")]
        public async Task <IActionResult> GetAll()
        {
            var categories = await _categorySarvice.GetAllCategories();
            return Ok(new
            {
                data= categories,
                _localizer["Success"].Value
            });
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _categorySarvice.GetCategory(c => c.Id == id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categorySarvice.DeleteCategory(id);
            if (!deleted)
            {
                return NotFound(new { message = _localizer["NotFound"].Value });
            }
            return Ok(new { message = _localizer["Success"].Value });
        }



    }
}
