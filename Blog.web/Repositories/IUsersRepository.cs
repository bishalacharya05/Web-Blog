using Microsoft.AspNetCore.Identity;

namespace Blog.web.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
