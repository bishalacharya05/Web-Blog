namespace Blog.web.Models.Domain
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDecription { get; set; }
        public string FeaturedImageUrl{ get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }

        //this is means that we can add many tags to 1 blog
        //basically this is many to many relationships
        public ICollection<Tag> Tags { get; set; }
      
    }
}
