using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.DTO.Response
{
    public class OrderResponse
    {
        public int Id {  get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }

        public decimal AmountBaid { get; set; }

        public OrderStatusEnum OrderStatus { get; set; } 
        public PaymentMethodeenum PaymentMethode { get; set; }
        public DateTime OrderDate {  get; set; }
        //public List <OrderItemResponse> OrderItems { get; set; }
    }
}
