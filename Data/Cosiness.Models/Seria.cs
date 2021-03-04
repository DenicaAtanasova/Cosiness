namespace Cosiness.Models
{
    using System.Collections.Generic;

    public class Seria
    {
        public Seria()
        {
            this.Products = new HashSet<Product>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}