namespace Cosiness.Services.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Models.Enums;
    using Cosiness.Services.Data;
    using Cosiness.Services.Data.Tests.Common;
    using Cosiness.Services.Mapping;
    using Cosiness.Web.InputModels.Addresses;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using Xunit;

    public class AddressServiceTests
    {
        private readonly CosinessDbContext _context;
        private readonly IAddressService _addressesService;

        private string _townId;
        private string _townName = "Sofia";
        private string _addressId;

        public AddressServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new CosinessDbContext(options);

            SeedData();

            var townsService = new Mock<IBaseNameOnlyEntityService<Town>>();
            townsService.Setup(x => x.GetIdByNameAsync(_townName))
                .ReturnsAsync(_townId);

            _addressesService = new AddressService(_context, townsService.Object);

            AutoMapperConfig.RegisterMappings(
                typeof(AddressInputModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var address = new AddressInputModel
            {
                Town = new AddressTownInputModel { Name = _townName },
                Street = "Test street",
                AddressType = AddressType.Billing.ToString(),
                BuildingNumber = "10A"
            };

            var createdAddressId = await _addressesService.CreateAsync(address);
            var addressFromDb = await _context.Addresses
                .FirstOrDefaultAsync(x => x.Id == createdAddressId);

            Assert.Equal(address.Town.Name, addressFromDb.Town.Name);
            Assert.Equal(address.Street, addressFromDb.Street);
            Assert.Equal(address.AddressType, addressFromDb.AddresType.ToString());
            Assert.Equal(address.BuildingNumber, addressFromDb.BuildingNumber);
        }

        [Fact]
        public async Task UpdateAsync_ShouldWorkCorrectly()
        {
            var address = new AddressInputModel
            {
                Town = new AddressTownInputModel { Name = _townName },
                Street = "Updates street",
                AddressType = AddressType.Other.ToString(),
                BuildingNumber = "Initial number"
            };

            await _addressesService.UpdateAsync(_addressId, address);
            var addressFromDb = await _context.Addresses
                .FirstOrDefaultAsync(x => x.Id == _addressId);

            Assert.Equal(address.Street, addressFromDb.Street);
            Assert.Equal(address.BuildingNumber, addressFromDb.BuildingNumber);
            Assert.Equal(address.AddressType, addressFromDb.AddresType.ToString());
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var address = new AddressInputModel
            {
                Town = new AddressTownInputModel { Name = _townName },
                Street = "Test street",
                AddressType = AddressType.Billing.ToString(),
                BuildingNumber = "10A"
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _addressesService.UpdateAsync(incorrectId, address));
            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_addressesService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            Assert.NotEmpty(_context.Addresses);

            await _addressesService.DeleteAsync(_addressId);

            Assert.Empty(_context.Addresses);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenEmptyCollection()
        {
            await _addressesService.DeleteAsync(_addressId);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _addressesService.DeleteAsync(_addressId));
            var epectedMessage = ErrorMessage.GetEmptyCollectionMessage(_addressesService.GetType().Name);

            Assert.Equal(epectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _addressesService.DeleteAsync(incorrectId));
            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_addressesService.GetType().Name);

            Assert.Equal(string.Format(expectedMessage, incorrectId), exception.Message);
        }

        private void SeedData()
        {
            var town = _context.Towns.Add(
                new Town
                {
                    Name = _townName
                })
                .Entity;

            _townId = town.Id;

            var initialAddress = _context.Addresses.Add(
                new Address
                {
                    TownId = _townId,
                    Street = "Initial street",
                    BuildingNumber = "Initial number",
                    AddresType = AddressType.Other
                }).Entity;

            _addressId = initialAddress.Id;

            _context.SaveChanges();
        }
    }
}