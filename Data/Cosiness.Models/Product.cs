namespace Cosiness.Models
{
    using System;
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
            this.Orders = new HashSet<OrderProduct>();
            this.Reviews = new HashSet<Review>();
        }

        public string Id { get; set; }

        public string RefNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public Category Category { get; set; }

        public Seria Seria { get; set; }

        public decimal Price { get; set; }

        public Image Image { get; set; }

        public Characteristic Characteristic { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}