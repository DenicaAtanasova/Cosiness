namespace Cosiness.Models
{
    public class ProductColor
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string ColorId { get; set; }

        public Color Color { get; set; }
    }
}