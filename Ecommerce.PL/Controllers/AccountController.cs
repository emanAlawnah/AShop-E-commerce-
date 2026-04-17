using Ecommerce.BLL.Service;
using Ecommerce.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService) {
        _authenticationService = authenticationService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginRequest request)
        {
         var result = await _authenticationService.LoginAsync(request);
            return Ok(result);
        }

        [HttpGet("ConfirmEmail")]

        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var isCinfermd = await _authenticationService.ConfirmEmailAsync(token, userId);

           if(isCinfermd) return Ok(new { message = "success" });

            return BadRequest();
        }

    }
}
