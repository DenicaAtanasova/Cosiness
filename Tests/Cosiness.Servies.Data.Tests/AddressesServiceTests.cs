﻿namespace Cosiness.Servies.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Models.Enums;
    using Cosiness.Services.Data;
    using Cosiness.Services.Mapping;
    using Cosiness.Web.InputModels.Addresses;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using Xunit;

    public class AddressesServiceTests
    {
        private readonly CosinessDbContext _context;
        private readonly IAddressesService _addressesService;

        private string _townId;
        private string _townName = "Sofia";

        private string _addressId;

        private readonly string IncorrectIdMessage = "{0} - Incorrect id: {1}!";
        private readonly string EmptyCollectionMessage = "{0} - Collection is empty!";

        public AddressesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new CosinessDbContext(options);

            this.SeedData();

            var townsService = new Mock<IBaseNameOnlyEntitiesService<Town>>();
            townsService.Setup(x => x.GetIdByNameAsync(_townName))
                .ReturnsAsync(_townId);

            _addressesService = new AddressesService(_context, townsService.Object);

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

            Assert.Equal(addressFromDb.Id, createdAddressId);
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
            var expectedMessage = string.Format(IncorrectIdMessage, _addressesService.GetType().Name, incorrectId);

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
            var epectedMessage = string.Format(EmptyCollectionMessage, _addressesService.GetType().Name);

            Assert.Equal(epectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _addressesService.DeleteAsync(incorrectId));
            var expectedMessage = string.Format(IncorrectIdMessage, _addressesService.GetType().Name, incorrectId);

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