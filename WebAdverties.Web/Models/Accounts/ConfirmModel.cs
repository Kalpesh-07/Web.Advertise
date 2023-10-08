using System.ComponentModel.DataAnnotations;

namespace WebAdverties.Web.Models.Accounts
{
    public class ConfirmModel
    {
        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Code is Required")]
        public string Code { get; set; }
    }
}
