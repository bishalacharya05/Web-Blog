using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
