namespace Cosiness.Models
{
    using Common;
    using System;

    public class Review : BaseEntity<string>
    {
        public DateTime CreatedOn { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public string CreatorId { get; set; }

        public CosinessUser Creator { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}