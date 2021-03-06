﻿namespace Cosiness.Models
{
    using Common;

    using System;
    using System.Collections.Generic;

    public class Product : BaseEntity<string>
    {
        public Product()
        {
            this.Colors = new HashSet<ProductColor>();
            this.Materials = new HashSet<ProductMaterial>();
            this.Orders = new HashSet<OrderProduct>();
            this.Reviews = new HashSet<Review>();
        }

        public string RefNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }

        public string SetId { get; set; }

        public Set Set { get; set; }

        public string DimensionId { get; set; }

        public Dimension Dimension { get; set; }

        public Image Image { get; set; }

        public decimal Price { get; set; }

        public Storage Storage { get; set; }

        public ICollection<ProductColor> Colors { get; set; }

        public ICollection<ProductMaterial> Materials { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}