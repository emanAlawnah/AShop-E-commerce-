using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using Ecommerce.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _OrderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _OrderRepository = orderRepository;
        }
        public async Task<List<OrderResponse>> GetUserOrders(string userId)
        {
           var orders = await _OrderRepository.GetAllAsync(filter : o=>o.UserId == userId,
               includes : new[] 
               {
                   nameof(Order.OrderItems),
                   $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                   $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}"+
                   $".{nameof(Product.Translations)}",
               }
               );

            return orders.Adapt<List<OrderResponse>>();
        }  


        public async Task<orderDetailesResponse?> GetUserOrder(string userId,int orderId)
        {
            var order = await _OrderRepository.GetOne(
                filter: o => o.UserId == userId && o.Id == orderId,
                includes: new[]
                {
                    nameof(Order.OrderItems),
                   $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                   $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}"+
                   $".{nameof(Product.Translations)}",
                }
                );
            if(order == null ) return null;
            return order.Adapt<orderDetailesResponse>();
        }

        public async Task<bool> cancelOrder(string userId, int orderId)
        {
            var order = await _OrderRepository.GetOne(
               filter: o => o.UserId == userId && o.Id == orderId);
            if (order is null ) return false;
            if(order.OrderStatus != OrderStatusEnum.Pending) return false;
            order.OrderStatus = OrderStatusEnum.Cancelled;
            return await _OrderRepository.UpdateAsync( order );
        }

     

        public async Task<List<OrderResponse>> GetAllOrders(OrderStatusEnum status)
        {
            var orders = await _OrderRepository.GetAllAsync(
                filter: o => o.OrderStatus == status
                );
            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request)
        {
           var order = await _OrderRepository.GetOne(o=>o.Id == orderId);
            if(order.OrderStatus == OrderStatusEnum.Cancelled || order.OrderStatus == OrderStatusEnum.Deleverd) return false;

            if((int) request.Status != (int) order.OrderStatus+1)
                return false;

            order.OrderStatus = request.Status;
            return await _OrderRepository.UpdateAsync( order );
        }
    }
}
