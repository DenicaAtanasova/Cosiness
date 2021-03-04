namespace Cosiness.Models
{
    using System.Collections.Generic;

    public class Material
    {
        public Material()
        {
            this.Characteristics = new HashSet<Characteristic>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Characteristic> Characteristics { get; set; }
    }
}