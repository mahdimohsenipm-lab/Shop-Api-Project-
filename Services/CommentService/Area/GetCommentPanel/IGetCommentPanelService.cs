namespace Services.CommentService.Area.GetCommentPanel
{
    public interface IGetCommentPanelService
    {
        Task<GetCommentsAdminResult> Execute(GetCommentsAdminRequest request);
    }


}
