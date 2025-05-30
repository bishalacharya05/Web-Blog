namespace Blog.web.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        //this means we can add one tags in multiple blog post
        //basically this is many to many relationships
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
 