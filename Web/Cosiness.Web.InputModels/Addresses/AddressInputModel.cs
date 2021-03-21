namespace Cosiness.Web.InputModels.Addresses
{
    using AutoMapper;

    using Cosiness.Models;
    using Cosiness.Models.Enums;
    using Cosiness.Services.Mapping;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddressInputModel : IMapExplicitly
    {
        [Required]
        public string AddressType { get; set; }

        [Required]
        public AddressTownInputModel Town { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string BuildingNumber { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AddressInputModel, Address>()
                .ForMember(dest => dest.AddresType, opt => opt.MapFrom(src => Enum.Parse<AddressType>(src.AddressType)));
        }
    }
}