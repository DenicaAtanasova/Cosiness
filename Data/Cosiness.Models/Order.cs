namespace Cosiness.Models
{
    using Common;
    using Enums;

    using System;
    using System.Collections.Generic;

    public class Order : BaseEntity<string>
    {
        public Order()
        {
            this.Products = new HashSet<OrderProduct>();
        }

        public string RefNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DispatchedOn { get; set; }

        public DateTime? DeliveredOn { get; set; }

        public ICollection<OrderProduct> Products { get; set; }

        public CosinessUser Recipient { get; set; }

        public Address DeliveryAddress { get; set; }

        public Address BillingAddress { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Cost { get; set; }

        public decimal Discount { get; set; }

        public decimal DeliveryCost { get; set; }

        public decimal TotalCost { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}