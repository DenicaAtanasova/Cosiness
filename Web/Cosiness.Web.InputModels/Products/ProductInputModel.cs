namespace Cosiness.Web.InputModels.Products
{
    using AutoMapper;

    using Cosiness.Models;
    using Cosiness.Services.Mapping;
    using Cosiness.Web.InputModels.Attributes.Validation;

    using Microsoft.AspNetCore.Http;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class ProductInputModel : IMapExplicitly
    {
        [Required]
        public string RefNumber { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Set { get; set; }

        [Required]
        public string Dimension { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Quantity")]
        public int StorageQuantity { get; set; }

        [Required]
        [AllowedFormats(new string[] { ".jpg", ".png" })]
        public IFormFile Image { get; set; }

        public ICollection<string> Colors { get; set; }

        public ICollection<string> Materials { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Product, ProductInputModel>()
                .ForMember(dest => dest.Materials, opt => opt.MapFrom(
                    src => src.Materials
                    .Select(x => x.Material.Name)
                    .ToList()));
        }     
    }
}