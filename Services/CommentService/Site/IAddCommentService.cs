using Services.ViewModel.Site;

namespace Services.CommentService.Site
{
    public interface IAddCommentService
    {
        Task<AddCommentResponse> Execute(RequestAddComment request,CancellationToken cancellationToken);
    }
}
