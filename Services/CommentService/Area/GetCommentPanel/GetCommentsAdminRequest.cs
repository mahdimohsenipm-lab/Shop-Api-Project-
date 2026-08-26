using System.ComponentModel.DataAnnotations;

namespace Services.CommentService.Area.GetCommentPanel
{
    public class GetCommentsAdminRequest
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public bool? IsConfirmed { get; set; }

        public CommentSortType Sort { get; set; } = CommentSortType.PendingFirst;

    }
    public enum CommentSortType
    {
        [Display(Name = "در انتظار تایید")]
        PendingFirst,
        [Display(Name = "جدیدترین")]

        Newest,

        [Display(Name = "قدیمی ترین")]

        Oldest,
        [Display(Name = "بالاترین امتیاز")]

        HighestRate
    }


}
