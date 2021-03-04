namespace Cosiness.Models
{
    using System;
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.Products = new HashSet<ShoppingCartProduct>();
        }

        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string RecipientId { get; set; }

        public CosinessUser Recipient { get; set; }

        public ICollection<ShoppingCartProduct> Products { get; set; }
    }
}