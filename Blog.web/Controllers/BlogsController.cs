using Blog.web.Models.Domain;
using Blog.web.Models.ViewModel;
using Blog.web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly IBlogPostLikeRepository blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IBlogPostCommentRepository blogPostCommentRepository;
        private readonly UserManager<IdentityUser> userManager;

        public BlogsController(IBlogPostRepository blogPostRepository, 
            IBlogPostLikeRepository blogPostLikeRepository, 
            SignInManager<IdentityUser> signInManager,
            IBlogPostCommentRepository blogPostCommentRepository,
            UserManager<IdentityUser> userManager)
        {
            this.blogPostRepository = blogPostRepository;
            this.blogPostLikeRepository = blogPostLikeRepository;
            this.signInManager = signInManager;
            this.blogPostCommentRepository = blogPostCommentRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked=false;
            var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();
            if (blogPost != null)
            {
                
                var totallikes=await blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if(signInManager.IsSignedIn(User))
                {
                    //getting like for the blog of the certain user 
                    var likesForBlog=await blogPostLikeRepository.GetLikesForBlog(blogPost.Id);
                    var userId = userManager.GetUserId(User);
                    if (userId != null)
                    {
                      
                        var likeFromUser= likesForBlog.FirstOrDefault(x=> x.UserId==Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }

                //Get comments for blog
                var blogCommentsDomainModel=await blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogcomment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment

                    {
                        Description = blogcomment.Description,
                        DateAdded = blogcomment.DateAdded,
                        Username=(await userManager.FindByIdAsync(blogcomment.UserId.ToString())).UserName 
                        

                    });
                }


                  blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Content = blogPost.Content,
                    PageTitle = blogPost.PageTitle,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    PublishDate = blogPost.PublishDate,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags.ToList(),
                    TotalLikes = totallikes,
                    Liked = liked,
                    Comments=blogCommentsForView
                    
                };
            }


            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
        {
            if(signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModel.Id,
                    Description = blogDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded=DateTime.Now

                };
                await blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs" , new { urlHandle=blogDetailsViewModel.UrlHandle});
            }
            return Forbid();
            
        }
    }

}
