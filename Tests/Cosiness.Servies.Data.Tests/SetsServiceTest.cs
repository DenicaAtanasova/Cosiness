namespace Cosiness.Servies.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class SetsServiceTest
    {
        private readonly CosinessDbContext _context;
        private readonly ISetsService _setsService;
        private readonly string _setId;
        private readonly string IncorrectIdMessage = "SetsService - incorrect id: {0}";
        private readonly string InvalidParameterMessage = "SetsService - parameter cannot be null or empty!";

        public SetsServiceTest()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new CosinessDbContext(options);

            _setId = Guid.NewGuid().ToString();
            _context.Sets.Add(
                new Set
                {
                    Id = _setId,
                    Name = "Initial"
                });
            _context.SaveChanges();

            _setsService = new SetsService(_context);
        }

        [Fact]
        public async Task Create_ShouldWorkCorrectly()
        {
            var setName = "Galaxy";

            var createdSetId = await _setsService.CreateAsync(setName);
            var setFromDb = await _context.Sets.FirstOrDefaultAsync(x => x.Name == setName);
            Assert.Equal(setFromDb.Id, createdSetId);

            var setCount = await _context.Sets.CountAsync();
            var expectedCount = 2;
            Assert.Equal(expectedCount, setCount);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Create_ShouldThrowWhenNameNullOrEmpty(string name)
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                 async () => await _setsService.CreateAsync(name));

            Assert.Equal(InvalidParameterMessage, exception.Message);
        }

        [Fact]
        public async Task Update_ShouldWorkCorrectly()
        {
            var updatedSetName = "Updated";
            await _setsService.UpdateAsync(_setId, updatedSetName);

            var updatedSet = await _context.Sets.FirstOrDefaultAsync();

            Assert.Equal(updatedSetName, updatedSet.Name);
        }

        [Fact]
        public async Task Update_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var updatedSetName = "Incorrect Id";

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _setsService.UpdateAsync(incorrectId, updatedSetName));

            Assert.Equal(string.Format(IncorrectIdMessage, incorrectId), exception.Message);
        }

        [Fact]
        public async Task Delete_ShouldWorkCorrectly()
        {
            await _setsService.DeleteAsync(_setId);
            Assert.Empty(_context.Sets);
        }

        [Fact]
        public async Task Delete_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _setsService.DeleteAsync(incorrectId));

            Assert.Equal(string.Format(IncorrectIdMessage, incorrectId), exception.Message);
        }
    }
}