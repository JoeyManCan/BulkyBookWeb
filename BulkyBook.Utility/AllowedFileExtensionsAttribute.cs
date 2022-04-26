using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Utility
{
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedFileExtensions;
        public AllowedFileExtensionsAttribute(string[] allowedFileExtensions)
        {
            _allowedFileExtensions = allowedFileExtensions;
        }

        protected override ValidationResult? IsValid(object? value, 
            ValidationContext validationContext)
        {
            try
            {
                var file = (IFormFile?)value;
                if(file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if(!_allowedFileExtensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage(extension.ToLower()));
                    }

                    return ValidationResult.Success;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return new ValidationResult("no file has been provided");
        }

        public string GetErrorMessage(string extension)
        {
            return $"The extension {extension} is not allowed";
        }
    }
}
