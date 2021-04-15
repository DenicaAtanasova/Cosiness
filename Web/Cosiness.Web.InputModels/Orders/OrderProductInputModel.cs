namespace Cosiness.Web.InputModels.Orders
{
    using Cosiness.Models;
    using Cosiness.Services.Mapping;

    public class OrderProductInputModel : IMapTo<OrderProduct>
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}