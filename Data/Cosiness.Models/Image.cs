namespace Cosiness.Models
{
    using Common;

    public class Image : BaseEntity<string>
    {
        public string Url { get; set; }

        public string Caption { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}