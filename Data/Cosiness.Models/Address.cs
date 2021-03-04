namespace Cosiness.Models
{
    using Enums;

    public class Address
    {
        public string Id { get; set; }

        public AddressType AddresType { get; set; }

        public Town Town{ get; set; }

        public string Street { get; set; }

        public string BuildingNumber { get; set; }
    } 
}