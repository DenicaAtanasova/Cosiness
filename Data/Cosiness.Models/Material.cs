namespace Cosiness.Models
{
    using Common;
    using System.Collections.Generic;

    public class Material : BaseNamedEntity<string>
    {
        public Material()
        {
            this.Characteristics = new HashSet<Characteristic>();
        }

        public ICollection<Characteristic> Characteristics { get; set; }
    }
}