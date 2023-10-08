using System.ComponentModel.DataAnnotations;

namespace WebAdverties.Web.Models.Accounts
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(6,ErrorMessage ="Password must be at least 6 character long!")]
        [Display(Name = "Passwor")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm password do not match!")]
        public string ConfirmPassword { get; set; }
    }
}
