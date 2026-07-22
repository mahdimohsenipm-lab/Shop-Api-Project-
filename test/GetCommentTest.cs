using Data.Contracts;
using Data.Repositories;
using Entites.Products;
using Entites.Users;
using Moq;
using Services.CommentService.Site.GetComment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Helpers;

namespace test
{
    public class GetCommentTest
    {
        //private readonly Mock<IRepository<Comment>> _commentsRepositoryMock;
        //private readonly Mock<IGetCommentService> _service;
        //public GetCommentTest()
        //{
        //    _commentsRepositoryMock = new Mock<IRepository<Comment>>();
        //    _service = new Mock<IGetCommentService>();
        //}
    
        [Fact]
        public async Task Execute_Should_Return_Comments()
        {
            //Arrange
            var context = ApplicationDbContextFactory.Create();

            var repository = new Repository<Comment>(context);

            var service = new GetCommentService(repository);



            var user = new User
            {
                Id = 1,
                FullName = "Mahdi"
            };

            context.Set<User>().Add(user);

            var product = new Product
            {
                Id = 1,
                Name = "Laptop",
                Brand = "Asus",
                Description = "",
                Price = 100,
                Inventory = 10,
                Displayed = true,
                CategoriId = 1
            };

            context.Set<Product>().Add(product);

            var comment = new Comment
            {
                Id = 1,

                ProductId = 1,

                User = user,

                UserId = user.Id,   

                Text = "Good",

                Rate = 5,

                IsConfirmed = true,

                CreateTime = DateTime.Now
            };


            var reply = new Comment
            {
                Id = 2,

                ProductId = 1,

                Parent = comment,

                ParentId = comment.Id,

                User = user,

                UserId = user.Id,

                Text = "Reply",

                Rate = 5,

                IsConfirmed = true,

                CreateTime = DateTime.Now
            };


            comment.Replies.Add(reply);
            context.Set<Comment>().Add(comment);
            context.Set<Comment>().Add(reply);

            await context.SaveChangesAsync();


            // Act
            var result = await service.Execute(product.Id, CancellationToken.None);


            Assert.NotNull(result);
            Assert.Single(result.Comments);


            Assert.Equal("Mahdi", result.Comments.First().FullName);
            Assert.Equal("Good", result.Comments.First().Text);
            Assert.Equal(5, result.Comments.First().Rate);


            Assert.Single(result.Comments.First().Replies);

            Assert.Equal(
                "Reply",
                result.Comments.First().Replies.First().Text);



            Assert.Equal(1, result.TotalComment);
            Assert.Equal(5, result.AverageRate);
        }
    }
}
