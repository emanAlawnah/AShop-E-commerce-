using Ecommerce.DAL.Data;
using Ecommerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repository
{
    public class CartRepository :GenericRepository<Cart>,ICartRepository
    {
        public CartRepository(ApplicationDbContext context): base(context) 
        { 
        
        }
    }
}
