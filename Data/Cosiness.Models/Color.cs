namespace Cosiness.Models
{
    using Common;

    using System.Collections.Generic;

    public class Color : BaseNameOnlyEntity<string>
    {
        public Color()
        {
            this.Characteristics = new HashSet<Characteristic>();
        }

        public ICollection<Characteristic> Characteristics { get; set; }
    }
}