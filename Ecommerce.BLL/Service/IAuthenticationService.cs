using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public interface IAuthenticationService
    {
        Task <RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> ConfirmEmailAsync(string token, string userid);
        Task<ForgottPasswordResponse> RequestPasswordReset(ForgottPasswordRequest request);
        Task<ResetPasswordResponse> ResetPasswordASync(ResetPasswordRequest request);

    }
}
