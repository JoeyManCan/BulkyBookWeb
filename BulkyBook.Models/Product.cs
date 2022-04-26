using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Price for 1-50")]
        public double Price { get; set; }

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Price for 51-100")]
        public double Price50 { get; set; }

        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 10000)]
        public double Price100 { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedFileExtensions(new string[] {".jpg",".png"})]
        public string ImageUrl { get; set; }

        //the properties below create a foreign key relationship
        //EF is smart enough to understand that the Category type is a navigation property here
        //the naming is specially important to connect the CategoryId property
        //to Category and make it its foreign Id
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]//not mandatory, but good practice
        public Category Category { get; set; }

        //the properties below create a foreign key relationship
        //EF is smart enough to understand that the CoverType type is a navigation property here
        //the naming is specially important to connect the CoverTypeId property
        //to CoverType and make it its foreign Id 
        [Required]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]//not mandatory, but good practice
        public CoverType CoverType { get; set; }
    }
}
