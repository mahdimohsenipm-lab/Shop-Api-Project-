using Entites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.CommentService.Site;
using Services.ViewModel.Site;
using WebFramework.Filter;

namespace StoreTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class CommentController : Controller
    {
        private readonly IAddCommentService _addCommentService;
        private readonly UserManager<User> _userManager;
        public CommentController(IAddCommentService addCommentService, UserManager<User> userManager)
        {
            _addCommentService = addCommentService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Add(RequestAddComment request , CancellationToken cancellationToken)
        {
            string userid =  _userManager.GetUserId(User);
            if (userid==null)
            {
                return NotFound("کاربر پیدا نشد ثبت نام کنید");
            }

            request.UserId = userid;
            var result =await _addCommentService.Execute(request,cancellationToken);

            if (result.IsSuccess==false)
            {
                return BadRequest(result.Message);
            }
            else
            {
                return Ok(result.Message);
            }
                
        
        }
    }
}
