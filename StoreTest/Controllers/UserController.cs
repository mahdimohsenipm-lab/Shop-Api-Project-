using AutoMapper;
using Common.Utilities;
using Data.Contracts;
using Data.Repositories;
using Entites.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Jwt;
using Services.ViewModel.Area.Model.Dto;
using Services.ViewModel.Site;
using WebFramework.Filter;


namespace StoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IJwtServic jwtServic;
        private readonly IMapper mapper;

        public UserController(IUserRepository userRepository,
            RoleManager<Role> roleManager, UserManager<User> userManager,
            IJwtServic jwtServic,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.jwtServic = jwtServic;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            var user = await userRepository.TableNoTracking.ToListAsync(cancellationToken);

            if (user == null)
            {
                return NotFound();

            }
            return Ok(user);
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
            {
                return NotFound();

            }
            return user;

        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<ActionResult<User>> Created(UserDto userdto, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Age = userdto.Age,
                Email = userdto.Email,
                PasswordHash = SecurityHelper.GetSha256Hash(userdto.Password),
                UserName = userdto.Name,
                LoginTime = DateTime.Now

            };
            //await userRepository.AddAsync(user, cancellationToken);
            var result= await userManager.CreateAsync(user,userdto.Password);

            var result2 = await roleManager.CreateAsync(new Role
            {
                Name = "Admin",
                Descerption = "admin role"
            });

            var result3 = await userManager.AddToRoleAsync(user, "Admin");
            return user;
        }

        [HttpGet("[action]")]
        public IActionResult GetClaims()
        {
            var claims = User.Claims.Select(c => new
            {
                c.Type,
                c.Value
            });
            return Ok(claims);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public IActionResult AdminTest()
        {
            return Ok(new
            {
                Message = "You are admin"
            });
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<string>> Login([FromBody]LoginRequest request,CancellationToken cancellationToken)
        {
            //if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
            //    throw new Exception("OAuth flow is not password.");
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user==null)
                throw new Exception("یکی از موارد اشتباه است");


            bool ValidationPassword = await userManager.CheckPasswordAsync(user,request.Password);
            if (!ValidationPassword)
                throw new Exception("یکی از موارد اشتباه است");

            user.LoginTime = DateTime.Now;
            

            var jwt = await jwtServic.GenerateAsync(user);
            return Ok(jwt);
        }
        [HttpPost("[action]")]
        [AllowAnonymous]


        public async Task<ActionResult<string>> Register([FromBody]RequestRegister requestRegister ,CancellationToken cancellationToken)
        {
            var existingUserByName = await userManager.FindByNameAsync(requestRegister.UserName);
            if (existingUserByName != null)
            {
                return BadRequest("نام کاربری قبلاً انتخاب شده است.");
            }
          
            var existingUserByEmail = await userManager.FindByEmailAsync(requestRegister.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest("این ایمیل قبلاً ثبت‌نام شده است.");
            }
            var user = mapper.Map<User>(requestRegister);
            var result = await userManager.CreateAsync(user, requestRegister.Password);
            if (!result.Succeeded)
            {
                
                return BadRequest(result.Errors);
            }
            var token = await jwtServic.GenerateAsync(user);
            return Ok(token);
        }
      

        [HttpPut]

        public async Task<ActionResult> Update(int id,User user,CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.Age = user.Age;
            updateUser.Email = user.Email;
            updateUser.IsActive = user.IsActive;
            updateUser.LoginTime = user.LoginTime;

            await userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken,id);

           await userRepository.DeleteAsync(user ,cancellationToken);

            return Ok();


        }

    }
}
