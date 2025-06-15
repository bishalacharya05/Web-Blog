using System.ComponentModel.DataAnnotations;

namespace Blog.web.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string Username {  get; set; }

        [Required]
        [MinLength(6,ErrorMessage ="Password Must be at least of 6 character")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
