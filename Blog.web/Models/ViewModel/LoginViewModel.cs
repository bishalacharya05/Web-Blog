using System.ComponentModel.DataAnnotations;

namespace Blog.web.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Username {  get; set; }

        [Required]
        [MinLength(6,ErrorMessage="Password must be of at least 6 characters")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }

    }
}
