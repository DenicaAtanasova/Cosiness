namespace Cosiness.Servies.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class CategoriesServiceTest
    {
        private readonly CosinessDbContext _context;
        private readonly ICategoiesService _categoriesService;
        private readonly string _categoryId;

        private readonly string IncorrectIdMessage = "SetsService - incorrect id: {0}";
        private readonly string InvalidParameterMessage = "SetsService - parameter cannot be null or empty!";
        private readonly string EmptyCollectionMessage = "SetsService - Collection is empty!";

        public CategoriesServiceTest()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new CosinessDbContext(options);

            _categoryId = Guid.NewGuid().ToString();
            _context.Categories.Add(
                new Category
                {
                    Id = _categoryId,
                    Name = "Initial"
                });
            _context.SaveChanges();

            _categoriesService = new CategoriesService(_context);
        }

        [Fact]
        public async Task Create_ShouldWorkCorrectly()
        {
            var categoryName = "Galaxy";

            var createdCategoryId = await _categoriesService.CreateAsync(categoryName);
            var categoryFromDb = await _context.Categories
                .FirstOrDefaultAsync(x => x.Name == categoryName);

            Assert.Equal(categoryFromDb.Id, createdCategoryId);

            var categoryCount = await _context.Categories.CountAsync();
            var expectedCount = 2;
            Assert.Equal(expectedCount, categoryCount);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Create_ShouldThrowWhenNameNullOrEmpty(string name)
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                 async () => await _categoriesService.CreateAsync(name));

            Assert.Equal(InvalidParameterMessage, exception.Message);
        }

        [Fact]
        public async Task Update_ShouldWorkCorrectly()
        {
            var updatedCategoryName = "Updated";
            await _categoriesService.UpdateAsync(_categoryId, updatedCategoryName);

            var updatedCategory = await _context.Categories.FirstOrDefaultAsync();

            Assert.Equal(updatedCategoryName, updatedCategory.Name);
        }

        [Fact]
        public async Task Update_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var updatedCategoryName = "Incorrect Id";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _categoriesService.UpdateAsync(incorrectId, updatedCategoryName));

            Assert.Equal(string.Format(IncorrectIdMessage, incorrectId), exception.Message);
        }

        [Fact]
        public async Task Delete_ShouldWorkCorrectly()
        {
            Assert.NotEmpty(_context.Categories);

            await _categoriesService.DeleteAsync(_categoryId);
            Assert.Empty(_context.Categories);
        }

        [Fact]
        public async Task Delete_ShouldThrowWhenEmprtyCollection()
        {
            await _categoriesService.DeleteAsync(_categoryId);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _categoriesService.DeleteAsync(_categoryId));

            Assert.Equal(EmptyCollectionMessage, exception.Message);
        }

        [Fact]
        public async Task Delete_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _categoriesService.DeleteAsync(incorrectId));

            Assert.Equal(string.Format(IncorrectIdMessage, incorrectId), exception.Message);
        }
    }
}