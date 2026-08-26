namespace Services.CommentService.Area.GetCommentPanel
{
    public class CommentAdminDto
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public byte Rate { get; set; }

        public bool IsConfirmed { get; set; }

        public int? ParentId { get; set; }

        public DateTime CreateTime { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public string UserName { get; set; } = null!;
    }


}
