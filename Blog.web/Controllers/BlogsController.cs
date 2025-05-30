using Blog.web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        public async  Task<IActionResult> Index(string urlHandle)
        {
            var blogPost =await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            return View(blogPost);
        }
    }
}
