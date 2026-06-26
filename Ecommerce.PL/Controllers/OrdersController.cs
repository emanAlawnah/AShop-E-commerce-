using Ecommerce.BLL.Service;
using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.Models;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _OrderService;
        private readonly IStringLocalizer<SharedResources> _Localizer;

        public OrdersController(IOrderService orderService, IStringLocalizer<SharedResources> localizer)
        {
            _OrderService = orderService;
            _Localizer = localizer;
        }

        [HttpGet("")]

        public async Task<IActionResult> GetMyOrders()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Orders = await _OrderService.GetUserOrders(userId);
            return Ok( new { data = Orders });
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetMyOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var Order = await _OrderService.GetUserOrder(userId,id);
            return Ok(new { data = Order });
        }

          [HttpGet("admin")]
          //[Authorize (Roles ="admin")]
        public async Task<IActionResult> GetAllOrders(OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            var Orders = await _OrderService.GetAllOrders(status);
            return Ok(new { data = Orders });
        }

        [HttpPatch("admin/{id}/status")]

        public async Task<IActionResult> ChangeOrderStatus(int id, [FromBody] ChangeOrderStatusRequest request)
        {
            var result = await _OrderService.ChangeOrderStatus(id, request);
            if (!result)
                return BadRequest();
            return Ok();
        }
    }
}
