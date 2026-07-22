namespace Services.ViewModel.Site.Dto.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Text { get; set; } = null!;
        public byte Rate { get; set; }
        public DateTime CreateTime { get; set; }
        public int? ParentId { get; set; }
        public List<CommentReplyDto> Replies { get; set; } = new();
    }
}
