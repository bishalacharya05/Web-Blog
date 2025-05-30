using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.web.Models.ViewModel
{
    public class EditBlogPostRequest
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDecription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public IEnumerable<SelectListItem> Tags { get; set; }
        //Collect Tag
        //when we write string it can only select one tag
        //But when we write string[], we made an array of the string type Tag
        public string[] SelectedTags { get; set; } = Array.Empty<string>();

    }
}
