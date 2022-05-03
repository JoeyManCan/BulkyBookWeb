using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BulkyBook.Utility
{
    public class FileUpload
    {
        public string ErrorMessage { get; set; }
        public decimal FileSizeLimit { get; set; } = 550;

        public string Upload(IFormFile file)
        {
            try
            {
                var supportedExtensions = new[]
                {
                    "jpeg", "jpg", "png", "gif"
                };
                var fileExtension = Path.GetExtension(file.FileName).Substring(1);
                if (!supportedExtensions.Contains(fileExtension))
                {
                    ErrorMessage = "File Extension Is InValid - Only Upload Image File";
                }
                else if(file.Length > (FileSizeLimit * 1024))
                {
                    ErrorMessage = $"File size Should Be UpTo {FileSizeLimit} KB";
                }
                else
                {
                    ErrorMessage = "File Has Been Successfully Uploaded";
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Upload Container Should Not Be Empty or Contact Admin";

            }
            return ErrorMessage;

        }
    }
}
