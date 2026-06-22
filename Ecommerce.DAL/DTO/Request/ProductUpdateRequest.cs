using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.DTO.Request
{
    public class ProductUpdateRequest
    {
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? Quantity { get; set; }
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? SubImages { get; set; }= new List<IFormFile>();
        public List<IFormFile>? NewImages { get; set; } = new List<IFormFile>();

        public List<ProductTranslationRequest>? Translations { get; set; }=new List<ProductTranslationRequest>();
        public int? CategoryId { get; set; }
    }
}
