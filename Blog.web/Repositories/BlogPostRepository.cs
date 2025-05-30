using Blog.web.Data;
using Blog.web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blog.web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext blogDbContext;

        public BlogPostRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await blogDbContext.AddAsync(blogPost);
            await blogDbContext.SaveChangesAsync();
             return blogPost;

        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await blogDbContext.BlogPosts.FindAsync(id);
            if(existingBlog!=null)
            {
                blogDbContext.BlogPosts.Remove(existingBlog);
                await blogDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await blogDbContext.BlogPosts.Include(x=> x.Tags).ToListAsync();
        }

        public  async Task<BlogPost?> GetAsync(Guid id)
        {
            //this reterive the blogPost from the database and also reterive the Tags
            return await blogDbContext.BlogPosts.Include(x=> x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
           return await blogDbContext.BlogPosts.Include(x=> x.Tags).
                FirstOrDefaultAsync(x=> x.UrlHandle == urlHandle);    
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
           var existingBlog = await blogDbContext.BlogPosts.Include(x =>x.Tags)
                .FirstOrDefaultAsync( x => x.Id == blogPost.Id);

            if (existingBlog != null) 
            {
                
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle; 
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDecription = blogPost.ShortDecription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl= blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle= blogPost.UrlHandle;
                existingBlog.Visible= blogPost.Visible;
                existingBlog.PublishDate = blogPost.PublishDate;
                existingBlog.Tags = blogPost.Tags;
                 
                await blogDbContext.SaveChangesAsync();


                return existingBlog;

            }

            return null;
        }
    }
}
