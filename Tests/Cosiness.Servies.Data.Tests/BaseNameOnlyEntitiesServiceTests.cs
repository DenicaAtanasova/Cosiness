namespace Cosiness.Servies.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Models.Common;
    using Cosiness.Services.Data;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public abstract class BaseNameOnlyEntitiesServiceTests<TEntity>
        where TEntity : BaseNameOnlyEntity<string>, new()
    {
        private readonly CosinessDbContext _context;
        private readonly IBaseNameOnlyEntitiesService<TEntity> _entitiesService;
        private readonly string _entityId;

        private readonly string IncorrectIdMessage = "{0} - Incorrect id: {1}!";
        private readonly string InvalidParameterMessage = "{0} - Parameter cannot be null or empty!";
        private readonly string EmptyCollectionMessage = "{0} - Collection is empty!";

        protected BaseNameOnlyEntitiesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new CosinessDbContext(options);

            _entitiesService = new BaseNameOnlyEntitiesService<TEntity>(_context);

            _entityId = Guid.NewGuid().ToString();

            this.SeedData();
        }

        [Fact]
        public async Task GetIdByNameAsync_ShouldWorkCorrectly()
        {
            var entityName = "Galaxy";

            var createdEntityId = await _entitiesService.GetIdByNameAsync(entityName);
            var entityFromDb = await _context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Name == entityName);

            Assert.Equal(entityFromDb.Id, createdEntityId);

            var actualCount = await _context.Set<TEntity>().CountAsync();
            var expectedCount = 2;
            Assert.Equal(expectedCount, actualCount);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetIdByNameAsync_ShouldThrowWhenNameNullOrEmpty(string name)
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                 async () => await _entitiesService.GetIdByNameAsync(name));
            var expectedMessage = string.Format(InvalidParameterMessage, _entitiesService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }


        [Fact]
        public async Task UpdateAsync_ShouldWorkCorrectly()
        {
            var updatedEntityName = "Updated";
            await _entitiesService.UpdateAsync(_entityId, updatedEntityName);

            var updatedEntity = await _context.Set<TEntity>().FirstOrDefaultAsync();

            Assert.Equal(updatedEntityName, updatedEntity.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var updatedEntityName = "Incorrect Id";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _entitiesService.UpdateAsync(incorrectId, updatedEntityName));
            var expectedMessage = string.Format(IncorrectIdMessage, _entitiesService.GetType().Name, incorrectId);

            Assert.Equal(string.Format(expectedMessage, incorrectId), exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            Assert.NotEmpty(_context.Set<TEntity>());

            await _entitiesService.DeleteAsync(_entityId);
            Assert.Empty(_context.Categories);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenEmprtyCollection()
        {
            await _entitiesService.DeleteAsync(_entityId);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _entitiesService.DeleteAsync(_entityId));
            var epectedMessage = string.Format(EmptyCollectionMessage, _entitiesService.GetType().Name);

            Assert.Equal(epectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _entitiesService.DeleteAsync(incorrectId));
            var expectedMessage = string.Format(IncorrectIdMessage, _entitiesService.GetType().Name, incorrectId);

            Assert.Equal(string.Format(expectedMessage, incorrectId), exception.Message);
        }

        private void SeedData()
        {

            _context.Set<TEntity>().Add(
                new TEntity
                {
                    Id = _entityId,
                    Name = "Initial"
                });

            _context.SaveChanges();
        }
    }

    public class SetsServiceTests : BaseNameOnlyEntitiesServiceTests<Set> { }
    public class CategoriesServiceTests : BaseNameOnlyEntitiesServiceTests<Category> { }
    public class MaterialsServiceTests : BaseNameOnlyEntitiesServiceTests<Material> { }
    public class TownsServiceTests : BaseNameOnlyEntitiesServiceTests<Town> { }
    public class ColorsServiceTests : BaseNameOnlyEntitiesServiceTests<Color> { }
}