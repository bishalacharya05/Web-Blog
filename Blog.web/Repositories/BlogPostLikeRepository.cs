
using Blog.web.Data;
using Blog.web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly BlogDbContext blogDbContext;

        public BlogPostLikeRepository(BlogDbContext blogDbContext)
        {
            this.blogDbContext = blogDbContext;
        }

        public async Task<BlogPostLike> AddLikesForBlog(BlogPostLike blogPostLike)
        {
            await blogDbContext.BlogPostLikes.AddAsync(blogPostLike);
            await blogDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public  async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
           return await blogDbContext.BlogPostLikes.
                Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            var blogLikes=await blogDbContext.BlogPostLikes.CountAsync(x=> x.BlogPostId==blogPostId);
            return blogLikes;
        }
    }
}
