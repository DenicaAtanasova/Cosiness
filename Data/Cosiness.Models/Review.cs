namespace Cosiness.Models
{
    using Common;
    using System;

    public class Review : BaseEntity<string>
    {
        public DateTime CreatedOn { get; set; }

        public int Raiting { get; set; }

        public string Comment { get; set; }

        public CosinessUser Creator { get; set; }

        public Product Product { get; set; }
    }
}