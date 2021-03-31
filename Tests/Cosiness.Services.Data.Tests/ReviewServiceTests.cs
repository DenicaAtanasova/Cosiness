
namespace Cosiness.Services.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data.Tests.Common;
    using Cosiness.Services.Mapping;
    using Cosiness.Web.InputModels.Reviews;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using Xunit;

    public class ReviewServiceTests
    {
        private readonly CosinessDbContext _context;
        private readonly IReviewService _reviewService;

        private string _userId;
        private string _productId;
        private string _reviewId;

        public ReviewServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CosinessDbContext(options);

            this.SeedData();

            _reviewService = new ReviewService(_context);

            AutoMapperConfig.RegisterMappings(
                typeof(ReviewInputModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var review = new ReviewInputModel
            {
                Comment = "New comment",
                Rating = 2
            };

            var reviewId = await _reviewService.CreateAsync(_userId, _productId, review);

            var reviewFromDb = await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == reviewId);

            Assert.Equal(reviewFromDb.Comment, review.Comment);
            Assert.Equal(reviewFromDb.Rating, review.Rating);
            Assert.Equal(reviewFromDb.CreatorId, _userId);
            Assert.Equal(reviewFromDb.ProductId, _productId);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowWhenIncorrectProductId()
        {
            var review = new ReviewInputModel
            {
                Comment = "New comment",
                Rating = 2
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _reviewService.CreateAsync(_userId, Guid.NewGuid().ToString(), review));

            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_reviewService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message); 
        }

        [Fact]
        public async Task UpdateAsync_ShouldWorkCorrectly()
        {
            var review = new ReviewInputModel
            {
                Comment = "New comment",
                Rating = 2
            };

            await _reviewService.UpdateAsync(_reviewId, review);

            var reviewFromDb = await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == _reviewId);

            Assert.Equal(review.Comment, reviewFromDb.Comment);
            Assert.Equal(review.Rating, reviewFromDb.Rating);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowWhenIncorrectId()
        {
            var review = new ReviewInputModel
            {
                Comment = "New comment",
                Rating = 2
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _reviewService.UpdateAsync(Guid.NewGuid().ToString(), review));

            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_reviewService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            Assert.NotEmpty(_context.Reviews);

            await _reviewService.DeleteAsync(_reviewId);

            Assert.Empty(_context.Reviews);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenEMpryCollection()
        {
            await _reviewService.DeleteAsync(_reviewId);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _reviewService.DeleteAsync(_reviewId));
            var epectedMessage = ErrorMessage.GetEmptyCollectionMessage(_reviewService.GetType().Name);

            Assert.Equal(epectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _reviewService.DeleteAsync(incorrectId));
            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_reviewService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        private void SeedData()
        {
            var user = new CosinessUser
            {
                UserName = "Denica"
            };
            _context.Users.Add(user);
            _userId = user.Id;

            var product = new Product
            {
                CreatedOn = DateTime.UtcNow
            };
            _context.Products.Add(product);
            _productId = product.Id;

            var review = new Review
            {
                CreatedOn = DateTime.UtcNow,
                ProductId = _productId,
                CreatorId = _userId
            };
            _context.Reviews.Add(review);
            _reviewId = review.Id;

            _context.SaveChanges();
        }
    }
}