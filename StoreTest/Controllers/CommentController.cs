using Data.Contracts;
using Entites.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.CommentService.Site.AddComent;
using Services.CommentService.Site.GetComment;
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
        private readonly IGetCommentService _getCommentService;
        public CommentController(IAddCommentService addCommentService,
            UserManager<User> userManager,IGetCommentService getCommentService
           )
        {
            _addCommentService = addCommentService;
            _userManager = userManager;
            _getCommentService = getCommentService;

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int productId, CancellationToken cancellationToken)
        {
            var Comment = await _getCommentService.Execute(productId,cancellationToken);
            if (Comment==null)
            {
                return NotFound();
            }
            return Ok(Comment);
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
