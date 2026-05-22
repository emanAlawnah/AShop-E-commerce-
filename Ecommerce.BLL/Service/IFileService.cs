using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public interface IFileService
    {
        Task<string?> UplodeAsync(IFormFile file);
        void Delete(string fileName);

    }
}
