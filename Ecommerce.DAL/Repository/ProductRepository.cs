using Ecommerce.DAL.Data;
using Ecommerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repository
{
    public class ProductRepository :GenericRepository<Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) :base(context) { }
    }
}
