using Ecommerce.BLL.Service;
using Ecommerce.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _CheckoutService;

        public CheckoutsController(ICheckoutService checkoutService)
        {
            _CheckoutService = checkoutService;
        }

        [HttpPost("")]

        public async Task<IActionResult> payment([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var responce = await _CheckoutService.ProcessCheckout(userId, request);
            if (!responce.Success)
                return BadRequest(responce);

            return Ok(responce); 
        }

        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Succes([FromQuery] string sessionId)

        {
            var resulte = await _CheckoutService.HandelSuccess(sessionId);
            return Ok(new { massage = "success", sessionId = sessionId });
        }
    }
}
