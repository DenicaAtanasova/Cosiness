namespace Cosiness.Models
{
    using Common;

    using System.Collections.Generic;

    public class Characteristic : BaseEntity<string>
    {
        public Characteristic()
        {
            this.Colors = new HashSet<Color>();
        }

        public Material Material { get; set; }

        public string Dimension { get; set; }

        public ICollection<Color> Colors { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}