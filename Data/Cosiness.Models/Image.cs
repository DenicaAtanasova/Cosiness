namespace Cosiness.Models
{
    public class Image
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string Caption { get; set; } //Name

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}