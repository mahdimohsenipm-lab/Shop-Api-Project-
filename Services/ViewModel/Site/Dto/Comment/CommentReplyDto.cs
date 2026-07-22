namespace Services.ViewModel.Site.Dto.Comment
{
    public class CommentReplyDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Text { get; set; } = null!;

        public DateTime CreateTime { get; set; }
    }
}
