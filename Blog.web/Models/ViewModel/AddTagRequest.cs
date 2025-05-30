//View modwl are created for the specific purpose to render the data only on the page not on the database.
//To store data to the database we use actual models that is why we didnot include id in View model
//When we submit the form the form data are filled in the View model and EF core maps the data into the actual model

namespace Blog.web.Models.ViewModel
{
    public class AddTagRequest
    {
        public string Name {  get; set; }
        public string DisplayName { get; set; }
    }
}
