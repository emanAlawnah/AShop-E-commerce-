using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.DTO.Response
{
    public class LoginResponse
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string? AccessToken { get; set; }
    }
}
