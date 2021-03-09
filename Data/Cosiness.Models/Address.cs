namespace Cosiness.Models
{
    using Common;
    using Enums;

    public class Address : BaseEntity<string>
    {
        public AddressType AddresType { get; set; }

        public Town Town{ get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }
    } 
}