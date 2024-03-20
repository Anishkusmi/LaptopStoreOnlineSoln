using System.ComponentModel.DataAnnotations;

namespace LaptopStoreOnline.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ComfirmPassword { get; set; }
        public string Address { get; set; }
    }
}
