namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Models.Enums;
    using Cosiness.Services.Mapping;
    using Cosiness.Web.InputModels.Orders;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        private readonly decimal _minCostForFreeDelivery = 90M;
        private readonly decimal _deliveryCost = 10M;

        private readonly CosinessDbContext _context;
        private readonly IShoppingCartService _shoppingCartService;

        public OrderService(
            CosinessDbContext context,
            IShoppingCartService shoppingCartService)
        {
            _context = context;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<string> CreateAsync(OrderInputModel inputModel)
        {
            await _shoppingCartService.ClearProductsAsync(inputModel.RecipientId);

            var order = inputModel.Map<OrderInputModel, Order>();

            order.CreatedOn = DateTime.UtcNow;
            order.Status = OrderStatus.Processing;
            order.Cost = inputModel.Products
                .Select(x => x.Price * x.Quantity)
                .Sum();

            if (order.Cost < _minCostForFreeDelivery)
            {
                order.DeliveryCost = _deliveryCost;
            }
            order.TotalCost = order.Cost * order.Discount + order.DeliveryCost;
            order.RefNumber = await GenerateRefNumber();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.Id;
        }

        //TODO Make separate Dispatch, Reject and Deliver order methods
        public async Task UpdateStatusAsync(string id, string status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(x => x.Id == id);
            order.Status = Enum.Parse<OrderStatus>(status);

            _context.Orders.Update(order);

            await _context.SaveChangesAsync();
        }

        private async Task<string> GenerateRefNumber()
        {
            var date = DateTime.UtcNow;
            var datePattern = $"{date.Year}{date.Month}{date.Day}";

            var lastOrderRefNumber = await _context.Orders
                .Where(x => Regex.IsMatch(x.RefNumber, $@"^{datePattern}.*"))
                .OrderByDescending(x => x.RefNumber)
                .Select(x => x.RefNumber)
                .FirstOrDefaultAsync();

            var currentOrderRefNumber = 0;

            if (lastOrderRefNumber is not null)
            {
                currentOrderRefNumber = int.Parse(lastOrderRefNumber.Substring(lastOrderRefNumber.Length - 3));
            }

            currentOrderRefNumber += 1;

            return $"{datePattern}{currentOrderRefNumber:D4}";
        }
    }
}