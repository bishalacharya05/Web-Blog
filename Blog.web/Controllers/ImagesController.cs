using System.Net;
using Blog.web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpGet]

        public IActionResult Uploads()
        {
            return Ok("this is get method");
        }
        [HttpPost]
        public  async Task<IActionResult> UploadsAsync( IFormFile file)
        {
           //calling the repository
              var imageUrl= await imageRepository.UploadAsync(file);
            if (imageUrl == null)
            {
                return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = imageUrl});

        }
    }
}
