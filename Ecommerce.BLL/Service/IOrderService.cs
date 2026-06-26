using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetUserOrders(string userId);
        Task<orderDetailesResponse?> GetUserOrder(string userId, int orderId);
        Task<bool> cancelOrder (string userId,int orderId);

        Task <List<OrderResponse>> GetAllOrders(OrderStatusEnum status);
        Task <bool> ChangeOrderStatus (int orderId,ChangeOrderStatusRequest request);
    }
}
