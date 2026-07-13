using Entites.Common;
using Entites.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entites.Users
{
    public class Comment : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public User User { get; set; }
        public string Text { get; set; } = null!;
        public byte Rate { get; set; }
        public bool IsConfirmed { get; set; }
        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }

    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(x => x.Rate)
                .HasMaxLength(5)
                .IsRequired();

            builder.Property(x => x.Text)
                .HasMaxLength(1000)
                .IsRequired();

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Replies)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
