namespace Cosiness.Models
{
    using Common;

    using System.Collections.Generic;

    public class Seria : BaseNamedEntity<string>
    {
        public Seria()
        {
            this.Products = new HashSet<Product>();
        }

        public ICollection<Product> Products { get; set; }
    }
}