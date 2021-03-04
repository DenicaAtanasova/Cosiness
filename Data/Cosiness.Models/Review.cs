namespace Cosiness.Models
{
    using System;

    public class Review
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Raiting { get; set; }

        public string Comment { get; set; }

        public CosinessUser Creator { get; set; }

        public Product Product { get; set; }
    }
}