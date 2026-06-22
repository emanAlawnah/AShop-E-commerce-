using Ecommerce.BLL.Service;
using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.Repository;
using Ecommerce.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Localization;

namespace Ecommerce.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ProductService;
        private readonly IStringLocalizer<SharedResources> _Localizer;

        public ProductsController(IProductService productService, IStringLocalizer<SharedResources> localizer)
        {
            _ProductService = productService;
            _Localizer = localizer;
        }
        [HttpGet("")]

        public async Task<IActionResult> Index([FromQuery]PaginationRequest request)
        {
            var products = await _ProductService.GetAllProductsAsync(request);
            return Ok(
                new { data = products });
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> Index(int id)
        {
            var product = await _ProductService.GetProduct(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(
                new { data = product });
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            await _ProductService.CreateProduct(request);
            return Ok();
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async  Task<IActionResult> Update(int id,[FromForm] ProductUpdateRequest request)
        {
            var updated = await _ProductService.UpdateProductAsync(id, request);
            if (!updated) return BadRequest();
            return Ok();

        }

        [HttpDelete("{id}")]
        [Authorize]
        public  async Task<IActionResult> Delete(int id)
        {
          var deeleted=  await _ProductService.DeleteProduct(id);
            if (!deeleted) return BadRequest();

            return Ok();
        }
    }
}
