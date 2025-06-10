using Blog.web.Models.Domain;

namespace Blog.web.Models.ViewModel
{
    //this view model is for the like and comment in the blog post
    public class BlogDetailsViewModel
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
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public int TotalLikes { get; set; } 

        public bool Liked { get; set; } 

        public string CommentDescription { get; set; }

        public IEnumerable<BlogComment> Comments { get; set; } 
    }
}
  