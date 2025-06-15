//View modwl are created for the specific purpose to render the data only on the page not on the database.
//To store data to the database we use actual models that is why we didnot include id in View model
//When we submit the form the form data are filled in the View model and EF core maps the data into the actual model

using System.ComponentModel.DataAnnotations;

namespace Blog.web.Models.ViewModel
{
    public class AddTagRequest
    {
        [Required]
        public string Name {  get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
