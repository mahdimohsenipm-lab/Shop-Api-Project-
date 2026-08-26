using Data.Contracts;
using Entites.Users;
using Microsoft.EntityFrameworkCore;

namespace Services.CommentService.Area.GetCommentDetailPanel
{
    public class GetCommentDetailPanelService : IGetCommentDetailPanelService
    {
        private readonly IRepository<Comment> _commentRepository;

        public GetCommentDetailPanelService(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDetailDto?> Execute(
            int id,
            CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.TableNoTracking
                .Where(x => x.Id == id)
                .Select(x => new CommentDetailDto
                {
                    Id = x.Id,

                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,

                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    UserEmail = x.User.Email,

                    Text = x.Text,
                    Rate = x.Rate,

                    IsConfirmed = x.IsConfirmed,

                    ParentId = x.ParentId,
                    ParentText = x.Parent != null
                        ? x.Parent.Text
                        : null,

                    RepliesCount = x.Replies.Count,

                    CreateTime = x.CreateTime
                })
                .FirstOrDefaultAsync(cancellationToken);

            return comment;
        }
    }
}
