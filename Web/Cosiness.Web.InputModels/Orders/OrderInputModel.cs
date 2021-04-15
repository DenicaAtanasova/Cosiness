namespace Cosiness.Web.InputModels.Orders
{
    using AutoMapper;

    using Cosiness.Models;
    using Cosiness.Models.Enums;
    using Cosiness.Services.Mapping;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrderInputModel : IMapExplicitly
    {
        public ICollection<OrderProductInputModel> Products { get; set; }

        public string DeliveryAddressId { get; set; }

        public string BillingAddressId { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [Required]
        public string DeliveryMethod { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<OrderInputModel, Order>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => Enum.Parse<DeliveryMethod>(src.DeliveryMethod)));

            configuration.CreateMap<OrderInputModel, Order>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => Enum.Parse<PaymentMethod>(src.PaymentMethod)));
        }
    }
}