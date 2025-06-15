using Blog.web.Models.ViewModel;
using Blog.web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUsersRepository usersRepository;
        private readonly UserManager<IdentityUser> userManager;

        public AdminUsersController(IUsersRepository usersRepository, UserManager<IdentityUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await usersRepository.GetAll();

            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new Models.ViewModel.User
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    EmailAddress = user.Email
                });
            }

            return View(usersViewModel);
        }

        [HttpPost]

        public async Task<IActionResult> List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var identityResult=await userManager.CreateAsync(identityUser, request.Password);

            if (identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    //assigining roles to the user

                    var roles = new List<string> { "User" };
                    if (request.AdminRoleCheckBox)
                    {
                        roles.Add("Admin");
                    }

                    identityResult = await userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }

                }
            }
            return View();
        }

        //Only superAdmin user can delete the users
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
          var user =  await userManager.FindByIdAsync(id.ToString());

            if(user is not null)
            {
                var identityResult = await userManager.DeleteAsync(user);
                if(identityResult is not null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "AdminUsers");
                }
            }
             
            return View();
        }
    }
}
