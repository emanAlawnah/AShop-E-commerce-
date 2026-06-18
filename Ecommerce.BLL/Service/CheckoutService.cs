using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using Ecommerce.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Stripe.Checkout;
using Stripe;
using Microsoft.AspNetCore.Http;
using AppProduct = Ecommerce.DAL.Models.Product;

namespace Ecommerce.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _CartRepository;
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IOrderRepository _OrderRepository;
        private readonly ICartService _CartService;
        private readonly IProductRepository _ProductRepository;
        private readonly IEmailSender _EmailSender;

        public CheckoutService(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, 
         IHttpContextAccessor httpContextAccessor,IOrderRepository orderRepository, ICartService cartService,
         IProductRepository productRepository,IEmailSender emailSender)
        {
            _CartRepository = cartRepository;
            _UserManager = userManager;
            _HttpContextAccessor = httpContextAccessor;
            _OrderRepository = orderRepository;
            _CartService = cartService;
            _ProductRepository = productRepository;
            _EmailSender = emailSender;
        }

        public async Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request)
        {

            var cartItems = await _CartRepository.GetAllAsync(
                filter: c => c.UserId == userId,
                includes: new[] { nameof(Cart.Product),
                 $"{nameof(Cart.Product)}.{nameof(AppProduct.Translations)}"
                }

                );

            if (!cartItems.Any())
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "empty cart"
                };

            var user = await _UserManager.FindByIdAsync(userId);

            var city = request.City ?? user.City;
            if (city is null)
            {
                return new CheckoutResponse

                {
                    Success = false,
                    Error = "city is required"
                };
            }

            var street = request.Street ?? user.Street;
            if (street is null)
            {
                return new CheckoutResponse

                {
                    Success = false,
                    Error = "street is required"
                };
            }

            var PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            if (PhoneNumber is null)
            {
                return new CheckoutResponse

                {
                    Success = false,
                    Error = "PhoneNumber is required"
                };
            }

            foreach (var item in cartItems)
            {
                if (item.Count > item.Product.Quantity)
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = "dosn't hve enuugh stock"
                    };
            }

            var order = new Order()
            {
                UserId = userId,
                City = city,
                Street = street,
                PhoneNumber = PhoneNumber,
                PaymentMethode = request.PaymentMethode,
                AmountPaid = cartItems.Sum(c => c.Product.Price * c.Count),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                 ProductId = c.ProductId,
                 Quantity = c.Count,
                 UnitPrice = c.Product.Price,
                 TotalPrice = c.Product.Price *c.Count
                }
                ).ToList()
            };
             await _OrderRepository.CreateAsync( order );
            if (request.PaymentMethode == PaymentMethodeenum.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = null

                };
            }

            if (request.PaymentMethode == PaymentMethodeenum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    SuccessUrl = $"{_HttpContextAccessor.HttpContext.Request.Scheme}://{_HttpContextAccessor.HttpContext.Request.Host}/api/Checkouts/success?sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_HttpContextAccessor.HttpContext.Request.Scheme}://{_HttpContextAccessor.HttpContext.Request.Host}/checkout/cancel",
                    LineItems = new List<SessionLineItemOptions>()

                };

                foreach (var item in cartItems)
                {
                    options.LineItems.Add(
                         new SessionLineItemOptions
                         {
                             PriceData = new SessionLineItemPriceDataOptions
                             {
                                 Currency = "USD",
                                 ProductData = new SessionLineItemPriceDataProductDataOptions
                                 {
                                     Name = item.Product.Translations.FirstOrDefault(t=>t.Language=="en").Name,
                                 },
                                 UnitAmount = (long)(item.Product.Price * 100),
                             },
                             Quantity = item.Count,
                         }
                        );
                }
                var service = new SessionService();
                var session = service.Create(options);
                order.StripeSessionId = session.Id;
                await _OrderRepository.UpdateAsync(order);
                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url

                };
            }
            return new CheckoutResponse { Success = false, Error = "Invalied payment methode" };
        }

        public async Task<CheckoutResponse> HandelSuccess(string sessionId)
        {
            var order = await _OrderRepository.GetOne(o => o.StripeSessionId == sessionId,
                includes: new[]
                {
                    nameof(Order.OrderItems),
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(AppProduct.Translations)}"
                }
                );
            order.OrderStatus = OrderStatusEnum.paid;
            await _OrderRepository.UpdateAsync(order);

            await _CartService.clearCart(order.UserId);
            var  user = await _UserManager.FindByIdAsync(order.UserId);
            await _EmailSender.SendEmailAsync(user.Email, "order confirmed", "<h2>your order has been placed successfully <h2>");

            var lowStock = await _ProductRepository.DecreaseQuantityAsync(order.OrderItems);

            foreach (var item in lowStock) { 

            if(lowStock != null)
            {
                await _EmailSender.SendEmailAsync("alawnah.eman@gmail.com", "low stock alert", $"<h2>product{item.Translations.FirstOrDefault(t => t.Language == "en").Name} currenr quantity{item.Quantity}  <h2>");

            }

            }

            return new CheckoutResponse
            {
                Success = true,
                OrderId = order.Id
            };
        }
    }
}
