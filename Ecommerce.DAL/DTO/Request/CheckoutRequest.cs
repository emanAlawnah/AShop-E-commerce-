using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.DAL.DTO.Request
{
    public enum PaymentMethodeenum
    {
        Cash=1,Visa=2
    }
    public class CheckoutRequest
    {
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PhoneNumber { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodeenum PaymentMethode {  get; set; }
    }
}
