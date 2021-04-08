namespace Cosiness.Web.InputModels.Products
{
    using Cosiness.Web.InputModels.Attributes.Validation;

    using Microsoft.AspNetCore.Http;

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ProductInputModel
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
        [AllowedFormats(new string[] { ".jpg", ".png" })]
        public IFormFile Image { get; set; }

        public ICollection<string> Colors { get; set; }

        public ICollection<string> Materials { get; set; }
    }
}