namespace Cosiness.Services.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data.Tests.Common;
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class ShoppingCartServiceTests
    {
        private readonly CosinessDbContext _context;
        private readonly IShoppingCartService _shoppingCartService;

        private string _cheepProductId;
        private string _expensiveProductId;
        private string _shoppingCartId;
        private string _userId;
        public ShoppingCartServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new CosinessDbContext(options);

            _shoppingCartService = new ShoppingCartService(_context);

            SeedData();
        }

        [Fact]
        public async Task ClearAsync_ShouldWorkCorrectly()
        {
            var shoppingCartProductsBeforeClear= _context.ShoppingCartsProducts
                .Where(x => x.ShoppingCartId == _shoppingCartId);

            Assert.NotEmpty(shoppingCartProductsBeforeClear);

            await _shoppingCartService.ClearProductsAsync(_shoppingCartId);
            var shoppingCartProductsAfterClear = _context.ShoppingCartsProducts
                .Where(x => x.ShoppingCartId == _shoppingCartId);

            Assert.Empty(shoppingCartProductsAfterClear);
        }

        [Fact]
        public async Task CleardAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _shoppingCartService.ClearProductsAsync(incorrectId));
            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_shoppingCartService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        private void SeedData()
        {
            var cheepProduct = _context.Products.Add(
                new Product
                {
                    Price = 50M
                }).Entity;

            _cheepProductId = cheepProduct.Id;

            var expensiveProduct = _context.Products.Add(
                new Product
                {
                    Price = 250M
                }).Entity;

            _expensiveProductId = expensiveProduct.Id;

            var user = _context.Users.Add(
                new CosinessUser
                {
                    UserName = "Denica"

                }).Entity;

            _userId = user.Id;

            var shoppingCart = _context.ShoppingCarts.Add(
                new ShoppingCart
                {
                    RecipientId = _userId,
                    Products = new List<ShoppingCartProduct>
                    {
                        new ShoppingCartProduct
                        {
                            ProductId = _cheepProductId,
                            Quantity = 10
                        },
                        new ShoppingCartProduct
                        {
                            ProductId = _expensiveProductId,
                            Quantity = 2
                        },
                    }
                }).Entity;
            _shoppingCartId = shoppingCart.Id;

            _context.SaveChanges();
        }
    }
}