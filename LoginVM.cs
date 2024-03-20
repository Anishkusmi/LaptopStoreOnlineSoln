using System.ComponentModel.DataAnnotations;

namespace LaptopStoreOnline.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage ="User Name is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Passroed id Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remeber Me")]
        public bool RemeberMe { get; set; }
    }
}
