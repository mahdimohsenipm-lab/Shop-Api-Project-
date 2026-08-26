namespace Services.CommentService.Area.GetCommentDetailPanel
{
    public class CommentDetailDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;

        public string Text { get; set; } = null!;
        public byte Rate { get; set; }

        public bool IsConfirmed { get; set; }

        public int? ParentId { get; set; }
        public string? ParentText { get; set; }

        public int RepliesCount { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
