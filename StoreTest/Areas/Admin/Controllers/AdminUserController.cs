using AutoMapper;
using Common.Utilities;
using Data.Contracts;
using Entites.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Services.ViewModel.Area.Model.Dto;
using Services.ViewModel.Area.Model.Request;
using Services.ViewModel.Site;
using WebFramework.Filter;
using X.PagedList.Extensions;
using static StoreTest.Areas.Admin.Controllers.AdminUserController;

namespace StoreTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiResultFilter]
    [Authorize(Roles = "Admin")]



    public partial class AdminUserController : Controller
    {

        private readonly IUserRepository userRepository;
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public AdminUserController(IUserRepository userRepository,
            RoleManager<Role> roleManager, UserManager<User> userManager,IMapper mapper)
        {
            this.userRepository = userRepository;

            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        public IActionResult Index(int? page)
        {
            var user = userRepository.TableNoTracking.Select(x => new UserDto
            {
                Age = x.Age,
                IsActive = x.IsActive,
                Email = x.Email,
                Name = x.UserName,
                Id = x.Id,
                IsStatic=x.IsStatic
            }).ToPagedList(page??1 , 2);

            var result = new ResultDtoUser
            {
                UserList = user

            };

            //var role = roleManager.Roles.ToList();

           
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int UserId, string FullName, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, UserId);
            user.UserName = FullName;
            var result = userRepository.UpdateAsync(user, cancellationToken);

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int UserId, CancellationToken cancellationToken)
        {
            //    var user = await userRepository.Table.Include(x=>x.UserRole).FirstOrDefaultAsync(x=>x.Id==UserId,cancellationToken);
            //    await userRepository.DeleteAsync(user, cancellationToken);
            //    foreach (var item in user.UserRole)
            //    {
            //      await roleManager.DeleteAsync(item);

            //    }

            var user = userRepository.GetById(UserId);

            var role =await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, role);
            await userManager.DeleteAsync(user);
            return Ok();


        }
        [HttpPost]
        public async Task<IActionResult> UserSatusChange(int UserId, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, UserId);

            if (user == null)
            {
                return BadRequest("کاربر پیدا نشد");
            }
            await userRepository.UserSatusChange(user, cancellationToken);

            return Ok();


        }
        [HttpGet]
        public async Task<IActionResult> Serch(string serchkey)
        {
            var result = userRepository.TableNoTracking.Where(x => x.UserName.Contains(serchkey) || x.Email.Contains(serchkey))
                .Select(x => new UserDto
                {
                    Name = x.UserName,
                    Email = x.Email,
                    Id = x.Id,
                    IsActive = x.IsActive
                }).Take(10).OrderBy(x => x.Name);

            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RequestAddUser user, CancellationToken cancellationToken)
        {
            try
            {
                if (user == null)
                {

                    return BadRequest("مقادیر را درست ارسال کنید");

                }
                var newuser = mapper.Map<User>(user);
                var role =await roleManager.FindByIdAsync(user.RoleId.ToString());
                newuser.LoginTime = DateTime.Now;

                var createResult = await userManager.CreateAsync(newuser, user.PasswordHash);
                if (!createResult.Succeeded)
                {
                    return BadRequest("مقادیر را درست ارسال کنید");
                }
                await userManager.UpdateSecurityStampAsync(newuser);
                if (!string.IsNullOrEmpty(user.RoleId.ToString()))
                {
                   
                    var addToRoleResult = await userManager.AddToRoleAsync(newuser, role.Name);
                    if (!addToRoleResult.Succeeded)
                    {
                        return BadRequest("مشکلی پیش امده است");
                    }
                }
                return Ok();
               
            }
            catch (Exception )
            {
                return BadRequest("مشکلی پیش اومده");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(roleManager.Roles.ToList(), "Id", "Name");
            return View();
        }


    }
}
