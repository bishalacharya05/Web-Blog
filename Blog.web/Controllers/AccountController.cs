using Blog.web.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,

            };

            var identityResult =await userManager.CreateAsync(identityUser ,registerViewModel.Password);

            if (identityResult.Succeeded)
            {
                //assign this user the User role

                var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
                if (roleIdentityResult.Succeeded)

                {
                    //show success notification
                }

            }
            //show success notification
            return RedirectToAction("Register");

        }
         
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
         public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
           var signReasult= await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password,false,false);

            if (signReasult.Succeeded)
            {
                return RedirectToAction("Index", "Home");

            }
            //If return Back to Login page
            else
            return Ok("Envalid Password or Username");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
