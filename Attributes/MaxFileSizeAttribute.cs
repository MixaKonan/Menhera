using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Menhera.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
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
                        if (file.Length > _maxFileSize)
                        {
                            return new ValidationResult(GetSizeErrorMessage());
                        }
                    }
                    return new ValidationResult(GetNullErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        private string GetSizeErrorMessage()
        {
            return $"Максимальный размер файла может быть {_maxFileSize} байт.";
        }

        private string GetNullErrorMessage()
        {
            return "Файл пустой.";
        }
        
    }
}