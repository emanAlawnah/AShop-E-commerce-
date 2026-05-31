using Ecommerce.DAL.DTO.Request;
using Ecommerce.DAL.DTO.Response;
using Ecommerce.DAL.Models;
using Ecommerce.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _CartRepository;
        private readonly IProductRepository _ProductRepository;

        public CartService(ICartRepository cartRepository,IProductRepository productRepository)
        {
            _CartRepository = cartRepository;
            _ProductRepository = productRepository;
        }
        public async Task<bool> AddToCart(AddtoCartRequest request, string UserId)
        {
            var product = await _ProductRepository.GetOne(p => p.Id == request.ProductId);
            if (product is null) return false ;
            var ExistengItem = await _CartRepository.GetOne(
                c => c.ProductId == request.ProductId && c.UserId == UserId 
                );
           

            var currentCount = ExistengItem?.Count ??0;
            var newCount = currentCount + request.Count;
            if (newCount > product.Quantity) return false ;

                if (ExistengItem != null) {
                ExistengItem.Count = newCount;
                await _CartRepository.UpdateAsync(ExistengItem);
            }
            else
            {
                var cartItem = request.Adapt<Cart>();
                cartItem.UserId = UserId;
                await _CartRepository.CreateAsync(cartItem);
            }
            return true;
         }

        public async Task<List<CartResponse>> GetCart(string UserId) 
        {
            var items = await _CartRepository.GetAllAsync(
                filter: c => c.UserId == UserId,
                includes: new string[]
                {
                    nameof(Cart.Product),
                    $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"
                }
                );
            return items.Adapt<List<CartResponse>>();
        
        }

        public async Task <bool> RemoveItem(int ProductId, string UserId)
        {
            var item = await _CartRepository.GetOne(c=>c.ProductId == ProductId &&  c.UserId == UserId);
            if (item == null) return false;
            return await _CartRepository.DeleteAysnc(item);
        }

        public async Task<bool> clearCart(string UserId)
        {
            var items = await _CartRepository.GetAllAsync(

                filter: c=> c.UserId == UserId
                );

            if(!items.Any()) return false;
            await _CartRepository.DeleteRangeAsync(items);

            //foreach (var item in items) 
            //{
            //    await _CartRepository.DeleteAysnc(item);
            //}
            return true;
        }

        public async Task<bool> UbdateQuantity(int ProductId, int count, string UserId)
        {
            var item = await _CartRepository.GetOne(
                c => c.ProductId == ProductId && c.UserId == UserId
                );
            if (item == null) return false;
            var product =  await _ProductRepository.GetOne(p=>p.Id == ProductId);
            if(count > product.Quantity) return false;

            item.Count = count;
            return await _CartRepository.UpdateAsync(item);
        }
    }
}
