using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopStoreOnline.Models
{
    public class Laptop
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Brand Name")]
        public string Brand { get; set; }
        [Required]  
        public string Description { get; set; }
        public double Price { get; set; }
        [ValidateNever]
        public string Path { get; set; }
        [NotMapped]
        [Display(Name ="Choose Image")]
        public IFormFile ImagePath { get; set; }

    }
}
