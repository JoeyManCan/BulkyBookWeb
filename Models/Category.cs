using System.ComponentModel.DataAnnotations;

namespace BulkyBookWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(60)]
        public string Name { get; set; } = null!;
        [Range(0,30)]
        [Display(Name ="Display Order")]
        public int DisplayOrder { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
