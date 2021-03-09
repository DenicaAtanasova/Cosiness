namespace Cosiness.Models
{
    using Common;

    using System;
    using System.Collections.Generic;

    public class ShoppingCart : BaseEntity<string>
    {
        public ShoppingCart()
        {
            this.Products = new HashSet<ShoppingCartProduct>();
        }

        public DateTime CreatedOn { get; set; }

        public string RecipientId { get; set; }

        public CosinessUser Recipient { get; set; }

        public ICollection<ShoppingCartProduct> Products { get; set; }
    }
}