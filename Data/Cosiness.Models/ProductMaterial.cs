namespace Cosiness.Models
{
    public class ProductMaterial
    {
        public string productId { get; set; }

        public Product Product { get; set; }

        public string MaterialId { get; set; }

        public Material Material { get; set; }
    }
}
