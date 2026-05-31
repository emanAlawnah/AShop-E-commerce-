using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public interface ICartService
    {
        Task<bool> AddToCart(AddtoCartRequest request, string UserId);
        Task<List<CartResponse>> GetCart(string UserId);
        Task<bool> RemoveItem(int ProductId, string UserId);
        Task <bool> UbdateQuantity(int ProductId, int count, string UserId);
        Task<bool> clearCart(string UserId);
    }
}
