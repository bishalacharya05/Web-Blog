using Blog.web.Models.Domain;

namespace Blog.web.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotalLikes(Guid blogPostId);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
       

        Task<BlogPostLike> AddLikesForBlog(BlogPostLike blogPostLike);
       
    }
}
