using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Services.ViewModel.Site
{
    public class RequestAddComment
    {
        public int ProductId { get; set; }
        public byte Rate { get; set; }
        public string Text { get; set; } = null!;
        public int? ParentId { get; set; }
        public string? UserId { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class CommentConfiguration : IEntityTypeConfiguration<RequestAddComment>
    {
        public void Configure(EntityTypeBuilder<RequestAddComment> builder)
        {
            builder.Property(x => x.Rate)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(x => x.Text)
                .HasMaxLength(1000)
                .IsRequired();

 
        }
    }
}
