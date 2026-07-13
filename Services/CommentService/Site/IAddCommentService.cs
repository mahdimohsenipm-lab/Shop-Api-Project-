using Services.ViewModel.Site;

namespace Services.CommentService.Site
{
    public interface IAddCommentService
    {
        Task<AddCommentResponse> Eecute(RequestAddComment request,CancellationToken cancellationToken);
    }
}
