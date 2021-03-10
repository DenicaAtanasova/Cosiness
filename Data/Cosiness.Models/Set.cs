namespace Cosiness.Models
{
    using Common;

    using System.Collections.Generic;

    public class Set : BaseNamedEntity<string>
    {
        public Set()
        {
            this.Products = new HashSet<Product>();
        }

        public ICollection<Product> Products { get; set; }
    }
}