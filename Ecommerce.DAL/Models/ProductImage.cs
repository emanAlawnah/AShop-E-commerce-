using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }
        public int ProductID { get; set; }
        public Product product { get; set; }
    }
}
