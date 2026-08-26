using X.PagedList;

namespace Services.CommentService.Area.GetCommentPanel
{
    public class GetCommentsAdminResult
    {
        public IPagedList<CommentAdminDto> Comments { get; set; } = null!;

        public int TotalComments { get; set; }

        public int ConfirmedComments { get; set; }

        public int PendingComments { get; set; }

        public GetCommentsAdminRequest Request { get; set; } = null!;
    }


}
