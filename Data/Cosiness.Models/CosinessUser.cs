namespace Cosiness.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class CosinessUser : IdentityUser
    {
        public CosinessUser()
        {
            this.Orders = new HashSet<Order>();
            this.Addresses = new HashSet<Address>();
            this.Reviews = new HashSet<Review>();
        }
        public ICollection<Order> Orders { get; set; }

        public ICollection<Address> Addresses{ get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}