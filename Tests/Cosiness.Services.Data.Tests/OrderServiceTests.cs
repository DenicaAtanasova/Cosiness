namespace Cosiness.Services.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Models.Enums;
    using Cosiness.Services.Mapping;
    using Cosiness.Web.InputModels.Orders;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class OrderServiceTests
    {
        private readonly CosinessDbContext _context;
        private readonly IOrderService _orderService;

        private string _orderId;

        private string _userId;

        private string _cheepProductId;
        private string _expensiveProductId;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new CosinessDbContext(options);

            var shoppingCartService = new Mock<IShoppingCartService>();
            shoppingCartService
                .Setup(x => x.ClearProductsAsync(It.IsAny<string>()))
                .Callback((string recipientId) =>
                {
                    var shoppingCart = _context.ShoppingCarts
                    .FirstOrDefault(x => x.RecipientId == recipientId);

                    _context.ShoppingCartsProducts.RemoveRange(shoppingCart.Products);
                    _context.SaveChanges();
                });

            _orderService = new OrderService(_context, shoppingCartService.Object);

            SeedData();

            AutoMapperConfig.RegisterMappings(
                typeof(OrderInputModel).GetTypeInfo().Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var order = new OrderInputModel
            {
                Products = new List<OrderProductInputModel>
                {
                    new OrderProductInputModel
                    {
                        ProductId = _cheepProductId,
                        Price = 50M,
                        Quantity = 10
                    },
                    new OrderProductInputModel
                    {
                        ProductId = _expensiveProductId,
                        Price = 250M,
                        Quantity = 2
                    }
                },
                DeliveryMethod = "Home",
                PaymentMethod = "Cash",
                RecipientId = _userId
            };

            var orderId = await _orderService.CreateAsync(order);

            var orderFromDb = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            Assert.Equal(order.DeliveryMethod, orderFromDb.DeliveryMethod.ToString());
            Assert.Equal(order.PaymentMethod, orderFromDb.PaymentMethod.ToString());
            Assert.Equal(order.Products.Select(x => x.Price * x.Quantity).Sum(), orderFromDb.Cost);

            Assert.Equal(
                order.Products.Select(x =>
                new
                {
                    Id = x.ProductId,
                    Price = x.Price
                }),
                orderFromDb.Products.Select(x =>
                new
                {
                    Id = x.ProductId,
                    Price = x.Product.Price
                }));
        }

        [Theory]
        [InlineData(OrderStatus.Dispatched)]
        [InlineData(OrderStatus.Rejected)]
        [InlineData(OrderStatus.Delivered)]
        public async Task UpdateStatusAsync_ShouldWorkCorrectly(OrderStatus status)
        {
            await _orderService.UpdateStatusAsync(_orderId, status.ToString());

            var orderFromDb = await _context.Orders
                .FirstOrDefaultAsync(x => x.Id == _orderId);

            Assert.Equal(status, orderFromDb.Status);
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
                    UserName = "Denica",
                    ShoppingCart = new ShoppingCart
                    {
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
                    }
                }).Entity;
            _userId = user.Id;

            var date = DateTime.UtcNow;
            var order = _context.Orders.Add(
                new Order
                {
                    RecipientId = _userId,
                    Products = new List<OrderProduct>
                    {
                        new OrderProduct
                        {
                            ProductId = _cheepProductId,
                            Quantity = 10
                        },
                        new OrderProduct
                        {
                            ProductId = _expensiveProductId,
                            Quantity = 2
                        },
                    },
                    Status = OrderStatus.Processing,
                    RefNumber = $"{date.Year}{date.Month}{date.Day}0000"
        }).Entity;
            _orderId = order.Id;

            _context.SaveChanges();
        }
    }
}
