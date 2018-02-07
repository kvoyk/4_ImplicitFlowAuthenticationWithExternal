using System.ComponentModel.DataAnnotations;

namespace MLaw.Idp.Mvc.Models
{
    public class LogingViewModel
    {

        [Display(Name = "User Name")]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Your Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Validation Code")]
        public string ValidationCode { get; set; }
        public string RedirectUrl { get; set; }
        public string State { get; set; }
    }
}
