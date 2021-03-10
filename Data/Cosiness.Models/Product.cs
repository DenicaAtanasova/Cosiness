namespace Cosiness.Models
{
    using Common;

    using System;
    using System.Collections.Generic;

    public class Product : BaseEntity<string>
    {
        public Product()
        {
            this.Orders = new HashSet<OrderProduct>();
            this.Reviews = new HashSet<Review>();
        }

        public string RefNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public Category Category { get; set; }

        public Set Set { get; set; }

        public decimal Price { get; set; }

        public Image Image { get; set; }

        public Characteristic Characteristic { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}