using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CommentService.Area.GetCommentDetailPanel
{
    public interface IGetCommentDetailPanelService
    {
        Task<CommentDetailDto?> Execute(
       int id,
       CancellationToken cancellationToken);
    }
}
