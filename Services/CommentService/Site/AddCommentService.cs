using AutoMapper;
using Data.Contracts;
using Entites.Users;
using Microsoft.AspNetCore.Identity;
using Services.ViewModel.Site;

namespace Services.CommentService.Site
{
    public class AddCommentService : IAddCommentService
    {
        private readonly IRepository<Comment> _repository;
        private readonly IProductsRepository _productsRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;    

        public AddCommentService(IRepository<Comment> repository, IProductsRepository productsRepository
            ,IMapper mapper, UserManager<User> userManager)
        {
            _repository = repository;
            _productsRepository = productsRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<AddCommentResponse> Execute(RequestAddComment request, CancellationToken cancellationToken)
        {
             if (request is null)
                {
                return new AddCommentResponse 
                {
                    IsSuccess = false,
                    Message="مقادیر را اشتباه ارسال کردید"
                
                };
            }
            var product =await _productsRepository.GetByIdAsync(cancellationToken,request.ProductId);

            if (product==null)
            {
                return new AddCommentResponse
                {
                    IsSuccess = false,
                    Message = "محصولی یافت نشد"

                };
            }
            var result = _mapper.Map<Comment>(request);
            var user =await _userManager.FindByIdAsync(request.UserId);
            if (user==null)
            {
                return new AddCommentResponse
                {
                    IsSuccess = false,
                    Message = "کاربر پیدا نشد "

                };
            }

            result.IsConfirmed = false;
            result.User = user;
            await _repository.AddAsync(result, cancellationToken);
    

            return new AddCommentResponse
            {
                IsSuccess = true,
                Message = "نظر شما ثبت شد"
            };

        }
    }
}
