using Ecommerce.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Models
{

  public enum  OrderStatusEnum{
        Pending = 1,
        Approved =2,
        Shipped =3,
        Deleverd = 4,
        paid = 5,
        Cancelled =6,
        

        }
    public class Order
    {
        public int Id { get; set; }
        public PaymentMethodeenum PaymentMethode {  get; set; }

        public DateTime OrderDat {  get; set; } = DateTime.UtcNow;
        public DateTime? shippedAt { get; set; }
        public string? StripeSessionId { get; set; }
        public decimal? AmountPaid { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }=OrderStatusEnum.Pending;
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
