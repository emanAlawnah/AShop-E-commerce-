using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Ecommerce.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IEmailSender emailSender,IConfiguration configuration,
          IHttpContextAccessor httpContextAccessor  
            ) {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse()
                {
                    Success = false,
                    Message = "Eror",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            await _userManager.AddToRoleAsync(user, "user");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var emailUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/Account/ConfirmEmail?token={token}&userId={user.Id}";
             await _emailSender.SendEmailAsync(
              user.Email,"Welcome",$"<h1>Welcome {user.UserName}</h1>" +
              $""+
              $"<a href='{emailUrl}'>Confirm </a>"
            );
            return new RegisterResponse() { Success = true, Message = "success" };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (request == null)
            {
                return new LoginResponse { success = false, message = "Invalid request" };
            }
            if (user == null) {
                return new LoginResponse() { success = false, message = "Email is not valid" };

            }

            if (!await _userManager.IsEmailConfirmedAsync(user)) 
            { 
             return new LoginResponse { success = false, message = "email is not confirmed" };
            }
            var ISValiedPassword = await _userManager.CheckPasswordAsync(user,request.Password);
            
            if (!ISValiedPassword)
            {
                return new LoginResponse() { success = false, message = "Password is not valid" };
            }
            return new LoginResponse()
            {
                success = true,
                message = "Logged in successfuly",
                AccessToken = await GenerateAccessToken(user)

            };
    }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

           var token = new JwtSecurityToken(
           issuer: _configuration["Jwt:Issuer"],
           audience: _configuration["Jwt:Audience"],
           claims: userClaims,
           expires: DateTime.Now.AddDays(5),
           signingCredentials: credentials
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ConfirmEmailAsync(string token,string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user,token);
            if(!result.Succeeded) return false;
            return true;

        }

        public async Task<ForgottPasswordResponse> RequestPasswordReset(ForgottPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
              return  new ForgottPasswordResponse()
                {
                    success = false,
                    message = "email not found"
                };

              

            }
            var random = new Random();
            var code = random.Next(1000, 9999).ToString();
            user.CodeResetPassword = code;
            user.CodeResetPasswordExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(request.Email, "reset password", $"<p>code to reset your password is {code}</p>");
            return new ForgottPasswordResponse()
            {
                success = true,
                message = "Code send to your email successfully"
            };
        }

        public async Task<ResetPasswordResponse> ResetPasswordASync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResponse()
                {
                    success = false,
                    message = "email not found"
                };

            }

            else if(user.CodeResetPassword != request.Code)
            {
                return new ResetPasswordResponse()
                {
                    success = false,
                    message = "Invalied Code"
                };
            }

            else if (user.CodeResetPasswordExpiry < DateTime.UtcNow)
            {
                return new ResetPasswordResponse()
                {
                    success = false,
                    message = "Code Expierd"
                };
            }

            var isSamePassword = await _userManager.CheckPasswordAsync(user, request.NewPassword);

            if (isSamePassword) {

                return new ResetPasswordResponse()
                {
                    success = false,
                    message = "you cant use an old password"
                };

            }
            var token =await _userManager.GeneratePasswordResetTokenAsync(user);

           var result = await _userManager.ResetPasswordAsync(user,token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new ResetPasswordResponse()
                {
                    success = false,
                    message = "reset password faild"
                };

            }

            await _emailSender.SendEmailAsync(request.Email, "change password", $"<p>your password is changed</p>");

            return new ResetPasswordResponse()
            {
                success = true,
                message = " password changed successfuly"
            };


        }

    }
}
