using Data.Contracts;
using Entites.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CommentService.Area.DeleteComment
{
    public interface IDeleteCommentService
    {
        Task<bool> Execute(
            int id,
            CancellationToken cancellationToken);
    }
    public class DeleteCommentService : IDeleteCommentService
    {
        private readonly IRepository<Comment> _repository;

        public DeleteCommentService(IRepository<Comment> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Execute(
            int id,
            CancellationToken cancellationToken)
        {
            var comment = await _repository.GetByIdAsync(
                cancellationToken,
                id);

            if (comment == null)
                return false;

            var replies = await _repository.TableNoTracking
                .Where(x => x.ParentId == id)
                .ToListAsync(cancellationToken);

            if (replies.Any())
            {
                await _repository.DeleteRangeAsync(
                    replies,
                    cancellationToken,
                    saveNow: false);
            }

            await _repository.DeleteAsync(
                comment,
                cancellationToken);

            return true;
        }
    }
}
