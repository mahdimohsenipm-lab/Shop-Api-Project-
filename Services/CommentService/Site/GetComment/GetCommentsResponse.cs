using Services.ViewModel.Site.Dto.Comment;

namespace Services.CommentService.Site.GetComment
{
    public class GetCommentsResponse
    {
        public double AverageRate { get; set; }
        public int TotalComment { get; set; }

        public List<CommentDto> Comments { get; set; } = [];
    }
}
