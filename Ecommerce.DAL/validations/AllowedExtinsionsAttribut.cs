using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.validations
{
    public class AllowedExtinsions:ValidationAttribute
    {
        string[] _extensions = { ".jpg", ".wepb", ".png" };
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file){
                var extinsion = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extinsion)) 
                    return new ValidationResult($"Allowed extensions:{string.Join(",", _extensions)}");
                
            }
            return ValidationResult.Success;

        }
    }
}
