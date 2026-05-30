using Entites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebFramework.Filter;

namespace StoreTest.Areas.Admin.Controllers
{

    [ApiResultFilter]
    [Area("Admin")]
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AuthenticationController(RoleManager<Role> roleManager, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null ||
                string.IsNullOrWhiteSpace(loginDto.Email) ||
                string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("موارد را درست پر کنید");
            }
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return BadRequest("نام یا ایمیل اشتباه بود");
            }
            var password = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!password)
            {
                return BadRequest("نام یا ایمیل اشتباه بود");

            }
            if (!await userManager.IsInRoleAsync(user, "Admin"))
            {
                return BadRequest("عدم دسترسی");
            }
            var result = await signInManager.PasswordSignInAsync(user, loginDto.Password, true, false);
            if (!result.Succeeded)
            {
                return BadRequest("مشکلی در ثبت نام رخ داد");
            }
            return Ok();
        }

        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await signInManager.SignOutAsync();
            return Ok();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
