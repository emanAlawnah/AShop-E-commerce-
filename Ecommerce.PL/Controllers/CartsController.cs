using Ecommerce.BLL.Service;
using Ecommerce.DAL.DTO.Request;
using Ecommerce.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace Ecommerce.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _CartService;
        private readonly IStringLocalizer<SharedResources> _Localizer;

        public CartsController(ICartService cartService , IStringLocalizer<SharedResources> localizer)
        {
            _CartService = cartService;
            _Localizer = localizer;
        }

        [HttpPost("")]
       

        public async Task<IActionResult> AddTocart(AddtoCartRequest request)

        {
           var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           var result=  await _CartService.AddToCart(request, userId);
            if (!result) return BadRequest();
            return Ok(new

                {message = _Localizer["Success"].Value }
                );
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _CartService.GetCart(userId);
            return Ok(new {data = items});
        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int productId , [FromBody] updateCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var update = await _CartService.UbdateQuantity(productId,request.Count, userId);
            if (!update) return BadRequest();
            return Ok();

        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItem ([FromRoute]int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var removed = await _CartService.RemoveItem(productId,userId);

            if (!removed) return BadRequest();
            return Ok(new {  });
        }

        [HttpDelete("")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var clear = await _CartService.clearCart(userId);
            if (!clear) return BadRequest();
            return Ok();
        }

    }
}
