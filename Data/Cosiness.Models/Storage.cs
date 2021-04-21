using Cosiness.Models.Common;

namespace Cosiness.Models
{
    public class Storage : BaseEntity<string>
    {
        public string ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}