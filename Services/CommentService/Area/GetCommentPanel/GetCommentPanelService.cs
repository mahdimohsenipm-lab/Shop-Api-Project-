using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Entites.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList.EF;

namespace Services.CommentService.Area.GetCommentPanel
{

    public class GetCommentPanelService : IGetCommentPanelService
    {
        private readonly IRepository<Comment> _commentsRepository;
        private readonly IMapper _mapper;

        public GetCommentPanelService(
            IRepository<Comment> commentsRepository,
            IMapper mapper)
        {
            _commentsRepository = commentsRepository;
            _mapper = mapper;
        }

        public async Task<GetCommentsAdminResult> Execute(GetCommentsAdminRequest request)
        {
            var query = _commentsRepository.TableNoTracking;

            if (request.IsConfirmed.HasValue)
            {
                query = query.Where(x => x.IsConfirmed == request.IsConfirmed.Value);
            }

        

            var comments = await query
               
                .ProjectTo<CommentAdminDto>(_mapper.ConfigurationProvider).OrderByDescending(x=>x.CreateTime)
                .ToPagedListAsync(request.Page, request.PageSize);







            //query = request.Sort switch
            //{
            //    CommentSortType.Oldest=>query.OrderBy(x=>x.CreateTime),

            //    CommentSortType.PendingFirst => query.OrderBy(x => x.IsConfirmed==false)
            //    .ThenByDescending(x=>x.CreateTime),

            //    CommentSortType.Newest=>query.OrderByDescending(x=>x.CreateTime),

            //    CommentSortType.HighestRate=>query.OrderByDescending(x=>x.Rate),_=>query.OrderBy(x=>x.IsConfirmed)
            //    .ThenByDescending(x=>x.CreateTime),


            //};

            query = request.Sort switch
            {
                CommentSortType.Newest =>
                    query.OrderByDescending(x => x.CreateTime),

                CommentSortType.Oldest =>
                    query.OrderBy(x => x.CreateTime),

                CommentSortType.HighestRate =>
                    query.OrderByDescending(x => x.Rate),
                _ =>

                    query.OrderBy(x => x.IsConfirmed)
                         .ThenByDescending(x => x.CreateTime)
            };

            var result = new GetCommentsAdminResult();

            result.TotalComments = await query.CountAsync();

            result.ConfirmedComments = await query
                .CountAsync(x => x.IsConfirmed);

            result.PendingComments = await query
                .CountAsync(x => !x.IsConfirmed);

            result.Comments = await query
              
                .ProjectTo<CommentAdminDto>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(request.Page, request.PageSize);

            result.Request = request;

          



            return result;
        }
    }


}
