namespace Cosiness.Models
{
    using System.Collections.Generic;

    public class Characteristic
    {
        public Characteristic()
        {
            this.Colors = new HashSet<Color>();
        }

        public string Id { get; set; }

        public Material Material { get; set; }

        public string Dimension { get; set; }

        public ICollection<Color> Colors { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}