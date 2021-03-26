using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Menhera.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly List<string> _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions.ToList();
        }
        
    
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            if (value is List<IFormFile> list)
            {
                foreach (var file in list)
                {
                    if (file != null)
                    {
                        if (!_extensions.Contains(Path.GetExtension(file.FileName)))
                        {
                            return new ValidationResult(GetErrorMessage());
                        }
                    }
                    return new ValidationResult(GetNullErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Неподдерживаемый формат файла";
        }
        
        private string GetNullErrorMessage()
        {
            return "Файл пустой.";
        }
    }
}