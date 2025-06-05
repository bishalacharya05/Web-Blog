using Blog.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.web.Data
{
    public class BlogDbContext:DbContext
    { 
       
        public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options) 
        {
             
        }

        //this creats the tables on the database 
        public DbSet<BlogPost> BlogPosts { get; set; }  
        public DbSet<Tag> Tags { get; set; }

        public DbSet<BlogPostLike> BlogPostLikes { get; set; }
        public DbSet<BlogPostComment> BlogPostComments { get; set; }

    }
}
