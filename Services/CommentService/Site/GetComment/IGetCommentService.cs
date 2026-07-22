using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CommentService.Site.GetComment
{
    public interface IGetCommentService
    {
        Task<GetCommentsResponse> Execute(int productId, CancellationToken cancellationToken);
    }
}
