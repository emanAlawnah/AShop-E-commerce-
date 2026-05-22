using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Service
{
    public class FileService : IFileService
    {


        public async Task<string?> UplodeAsync(IFormFile file)
        {
           if(file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString()
                    +Path.GetExtension(file.FileName);

                var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                     "wwwroot",
                    "images",
                    fileName
                 );
                using (var stream = File.Create(filePath)) 
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }

            return null;

        }

        public void Delete(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            if (File.Exists(path)) File.Delete(path);
        }
    }
}
