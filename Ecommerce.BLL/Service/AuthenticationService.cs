using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Ecommerce.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IEmailSender emailSender) {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponse()
                {
                    success = false,
                    message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }
            await _userManager.AddToRoleAsync(user, "user");
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var emailUrl = $"https://localhost:7232/api/Account/ConfirmEmail?token={token}&userId={user.Id}";
             await _emailSender.SendEmailAsync(
              user.Email,"Welcome",$"<h1>Welcome {user.UserName}</h1>" +
              $""+
              $"<a href='{emailUrl}'>Confirm </a>"
            );
            return new RegisterResponse() { success = true, message = "success" };
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
                message = "Logged in successfuly"

            };
    }

        public async Task<bool> ConfirmEmailAsync(string token,string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return false;

            var result = await _userManager.ConfirmEmailAsync(user,token);
            if(!result.Succeeded) return false;
            return true;

        }

    }
}
