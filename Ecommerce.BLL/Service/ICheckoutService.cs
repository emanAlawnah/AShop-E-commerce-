using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public interface ICheckoutService
    {
         Task<CheckoutResponse> ProcessCheckout (string userId,CheckoutRequest request);
        Task<CheckoutResponse> HandelSuccess(string sessionId);


    }
}
