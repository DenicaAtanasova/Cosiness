namespace Cosiness.Services.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Models.Common;
    using Cosiness.Services.Data;
    using Cosiness.Services.Data.Tests.Common;
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public abstract class BaseNameOnlyEntityServiceTests<TEntity>
        where TEntity : BaseNameOnlyEntity<string>, new()
    {
        private readonly CosinessDbContext _context;
        private readonly IBaseNameOnlyEntityService<TEntity> _entitiesService;

        private readonly string _entityId;

        protected BaseNameOnlyEntityServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new CosinessDbContext(options);

            _entitiesService = new BaseNameOnlyEntityService<TEntity>(_context);

            _entityId = Guid.NewGuid().ToString();

            SeedData();
        }

        [Fact]
        public async Task GetIdByNameAsync_ShouldWorkCorrectly()
        {
            var entityName = "Galaxy";

            var createdEntityId = await _entitiesService.GetIdByNameAsync(entityName);
            var entityFromDb = await _context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Name == entityName);

            Assert.Equal(entityFromDb.Name, entityName);

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
            var expectedMessage = ErrorMessage.GetNullOrEmptyParameterMessage(_entitiesService.GetType().Name);

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
            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_entitiesService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
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
            var epectedMessage = ErrorMessage.GetEmptyCollectionMessage(_entitiesService.GetType().Name);

            Assert.Equal(epectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _entitiesService.DeleteAsync(incorrectId));
            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_entitiesService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
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

    public class SetsServiceTests : BaseNameOnlyEntityServiceTests<Set> { }
    public class CategoriesServiceTests : BaseNameOnlyEntityServiceTests<Category> { }
    public class MaterialsServiceTests : BaseNameOnlyEntityServiceTests<Material> { }
    public class TownsServiceTests : BaseNameOnlyEntityServiceTests<Town> { }
    public class ColorsServiceTests : BaseNameOnlyEntityServiceTests<Color> { }
    public class DimensionServiceTests : BaseNameOnlyEntityServiceTests<Dimension> { }
}