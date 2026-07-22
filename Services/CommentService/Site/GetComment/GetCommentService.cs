using AutoMapper;
using Data.Contracts;
using Entites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.ViewModel.Site.Dto.Comment;

namespace Services.CommentService.Site.GetComment
{
    public class GetCommentService : IGetCommentService
    {
        private readonly IRepository<Comment> _commentsRepository;
 

        public GetCommentService(IRepository<Comment> commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }
        public async Task<GetCommentsResponse> Execute(int productId, CancellationToken cancellationToken)
        {

            var comments = await _commentsRepository.TableNoTracking
           .Where(x => x.ProductId == productId &&
                  x.IsConfirmed &&
                  x.ParentId == null)
      .OrderByDescending(x => x.CreateTime)
      .Take(10)
      .Select(x => new CommentDto
      {
          Id = x.Id,
          FullName = x.User.FullName,
          Text = x.Text,
          Rate = x.Rate,
          CreateTime = x.CreateTime,

          Replies = x.Replies
              .Where(r => r.IsConfirmed).OrderByDescending(x=>x.CreateTime).Take(5)
              .Select(r => new CommentReplyDto
              {
                  Id = r.Id,
                  FullName = r.User.FullName,
                  Text = r.Text,
                  CreateTime = r.CreateTime
              }).ToList()
      })
      .ToListAsync(cancellationToken);


            var info = await _commentsRepository.TableNoTracking
    .Where(x => x.ProductId == productId &&
                x.IsConfirmed &&
                x.ParentId == null)
    .GroupBy(x => 1)
    .Select(x => new
    {
        TotalComment = x.Count(),
        AverageRate = x.Average(c => c.Rate)
    })
    .FirstOrDefaultAsync(cancellationToken);


            if (!comments.Any())
            {
                return new GetCommentsResponse
                {
                    Comments = [],
                    TotalComment = 0,
                    AverageRate = 0
                };


            }
          
            var result=new GetCommentsResponse 
            {
            Comments=comments,
            AverageRate= info.AverageRate,
            TotalComment= info.TotalComment

            };

            return result;
        }
    }
}
