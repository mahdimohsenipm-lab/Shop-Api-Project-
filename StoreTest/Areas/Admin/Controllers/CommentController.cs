using Data.Contracts;
using Entites.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.CommentService.Area.GetCommentDetailPanel;
using Services.CommentService.Area.GetCommentPanel;
using System.Threading.Tasks;
using WebFramework.Filter;

namespace StoreTest.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiResultFilter]
    [Authorize(Roles = "Admin")]

    public class CommentController : Controller
    {
       private readonly IGetCommentPanelService _getCommentPanelService;
        private readonly IRepository<Comment> _repository;
        private readonly IGetCommentDetailPanelService _getCommentDetailPanelService;

        public CommentController(IGetCommentPanelService getCommentPanelService,
            IRepository<Comment> repository,
            IGetCommentDetailPanelService getCommentDetailPanelService)
        {
            _getCommentPanelService = getCommentPanelService;
            _repository = repository;
            _getCommentDetailPanelService = getCommentDetailPanelService;
        }

        public async Task<IActionResult> Index(GetCommentsAdminRequest request)
        {
            var result =await _getCommentPanelService.Execute(request);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(
       int id,
       CancellationToken cancellationToken)
        {
            var result = await _getCommentDetailPanelService
                .Execute(id, cancellationToken);

            if (result == null)
                return NotFound();

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var comment = await _repository.GetByIdAsync(cancellationToken, id);
            if (comment == null)
            {
                return NotFound();
            }
           
            await _repository.DeleteAsync(comment, cancellationToken);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id, CancellationToken cancellationToken)
        {
            var commens = await _repository.GetByIdAsync(cancellationToken, id);
            if (commens == null) return NotFound();
            commens.IsConfirmed = !commens.IsConfirmed;
            await _repository.UpdateAsync(commens, cancellationToken);

            return Ok();

        }
    }
}
