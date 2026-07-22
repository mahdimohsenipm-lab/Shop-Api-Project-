using AutoMapper;
using Data.Contracts;
using Entites.Products;
using Entites.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.CommentService.Site.AddComent;
using Services.ViewModel.Site;
using System.Xml.Linq;

namespace test
{
    public class AddCommentServiceTests
    {
        private readonly Mock<IRepository<Comment>> _repository;
        private readonly Mock<IProductsRepository> _productsRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<UserManager<User>> _userManager;

        public AddCommentServiceTests()
        {
            _repository = new Mock<IRepository<Comment>>();
            _productsRepository = new Mock<IProductsRepository>();
            _mapper = new Mock<IMapper>();

            var store = new Mock<IUserStore<User>>();

            _userManager = new Mock<UserManager<User>>(
                store.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);
        }

        private AddCommentService CreateService()
        {
            return new AddCommentService(
                _repository.Object,
                _productsRepository.Object,
                _mapper.Object,
                _userManager.Object);
        }



        [Fact]
        public async Task Execute_Should_ReturnFailure_When_RequestIsNull()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = await service.Execute(null!, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("مقادیر را اشتباه ارسال کردید");
        }



        [Fact]
        public async Task Execute_Should_ReturnFailure_When_Product_NotFound()
        {
            // Arrange
            var service = CreateService();

            var request = new RequestAddComment
            {
                ProductId = 1,
                Rate = 5,
                Text = "Test",
                UserId = "1"
            };

            _productsRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), request.ProductId))
                .ReturnsAsync((Product?)null);

            // Act
            var result = await service.Execute(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("محصولی یافت نشد");
        }



        [Fact]
        public async Task Execute_Should_ReturnFailure_When_User_NotFound()
        {
            // Arrange
            var service = CreateService();

            var request = new RequestAddComment
            {
                ProductId = 1,
                Rate = 5,
                Text = "Test",
                UserId = "123"
            };

            _productsRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), request.ProductId))
                .ReturnsAsync(new Product());

            _mapper
                .Setup(x => x.Map<Comment>(request))
                .Returns(new Comment
                {
                    UserId = 123
                });

            _userManager
                .Setup(x => x.FindByIdAsync("123"))
                .ReturnsAsync((User)null!);

            // Act
            var result = await service.Execute(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be("کاربر پیدا نشد ");
        }


        [Fact]
        public async Task Execute_Should_AddComment_When_Request_IsValid()
        {
            // Arrange
            var service = CreateService();

            var request = new RequestAddComment
            {
                ProductId = 1,
                Rate = 5,
                Text = "نظر تستی",
                UserId = "123"
            };

            var comment = new Comment
            {
                UserId = 123
            };

            var user = new User();

            _productsRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<CancellationToken>(), request.ProductId))
                .ReturnsAsync(new Product());

            _mapper
                .Setup(x => x.Map<Comment>(request))
                .Returns(comment);

            _userManager
                .Setup(x => x.FindByIdAsync(comment.UserId.ToString()))
                .ReturnsAsync(user);

            _repository
    .Setup(x => x.AddAsync(
        It.IsAny<Comment>(),
        It.IsAny<CancellationToken>(),
        It.IsAny<bool>()))
    .Returns(Task.CompletedTask);

            // Act
            var result = await service.Execute(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be("نظر شما ثبت شد");

            _repository.Verify(x => x.AddAsync(
      It.IsAny<Comment>(),
      It.IsAny<CancellationToken>(),
      It.IsAny<bool>()),
      Times.Once);
        }
    }
}
