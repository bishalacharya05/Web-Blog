using Blog.web.Models.Domain;
using Blog.web.Models.ViewModel;
using Blog.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.web.Controllers
{
  
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Add()
        {
            //Getting the tags from the repository
            var tags= await tagRepository.GetAllAsync();
            
            var model = new AddBlogPostsRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
        {



            var blogPost = new BlogPost
            {
                Heading = addBlogPostsRequest.Heading,
                PageTitle = addBlogPostsRequest.PageTitle,
                ShortDecription = addBlogPostsRequest.ShortDecription,
                Content = addBlogPostsRequest.Content,
                FeaturedImageUrl = addBlogPostsRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostsRequest.UrlHandle,
                PublishDate = addBlogPostsRequest.PublishDate,
                Author = addBlogPostsRequest.Author,
                Visible = addBlogPostsRequest.Visible


            };
            //Mapping Tag from selected tags
            var selectedTags = new List<Tag>();
            foreach(var selectedTagId in addBlogPostsRequest.SelectedTags)
            {
                var selectedTagIdGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdGuid);
                if(existingTag!=null)
                {
                    selectedTags.Add(existingTag);
                }

            }
            //mapping tags back to the main model
            blogPost.Tags = selectedTags;
            
            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Index","Home"); 
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        { 
            //calling the repository
            var blogPosts = await blogPostRepository.GetAllAsync();
            return View(blogPosts);
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            //this reterive the blogPost from the Repository
            var blogPost = await blogPostRepository.GetAsync(id);
            //This id done to map the Tags to the edit operation
            var tagsDomainModel= await tagRepository.GetAllAsync();
            if (blogPost != null)
            {
                //Mapping the domain model or database date to the view model
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDecription = blogPost.ShortDecription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishDate = blogPost.PublishDate,
                    Visible = blogPost.Visible,
                    Author = blogPost.Author,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()

                };
                return View(model); 
            }


            return View(null);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //Now we map view model to again domain model
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDecription = editBlogPostRequest.ShortDecription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishDate = editBlogPostRequest.PublishDate,
                Visible = editBlogPostRequest.Visible,

            };

            //Mapping tags to the domain model
            //this line create the empty list to store the selectedTag fromt the database
            var selectedTags= new List<Tag>();
            //this line loop through the all the tag that user selected in the form an that tags are in the string format
            foreach( var selectedTag in editBlogPostRequest.SelectedTags)
            {
                //this line convert the tag into th Guid format and if the string is valid guid and result is stored into tag variable
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    //this line gets the actual tag from the database using the parsed Guid
                    var foundTag = await tagRepository.GetAsync(tag);
                    //And if the tag exixts and it is added to the selectedTags list
                    if (foundTag != null)
                    { 
                        selectedTags.Add(foundTag);
                    }
                }
            }
            //this line add the selected tags to the Main domain model
            blogPostDomainModel.Tags = selectedTags;

            var updateBlog = await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updateBlog != null)

            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Edit");
            }

        }

        
        [HttpPost]

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Interacting with Repository to delete blog post and tags
           var deletedBlogPost= await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if(deletedBlogPost!=null)
            {
                //Showing the success response
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit" ,new { editBlogPostRequest.Id } );
        }
    }
}
