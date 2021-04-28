namespace Cosiness.Web.ViewModels.Admin.Products
{
    using Cosiness.Models;
    using Cosiness.Services.Mapping;

    public class ProductAllViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string RefNumber { get; set; }

        public int StorageQuantity { get; set; }
    }
}