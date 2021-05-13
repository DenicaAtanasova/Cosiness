namespace Cosiness.Web.InputModels.Products
{
    using Cosiness.Models;
    using Cosiness.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProductEditInputModel : IMapFrom<Product>
    {
        [Required]
        public string RefNumber { get; set; }

        [Required]
        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [Required]
        [DisplayName("Set")]
        public string SetName { get; set; }

        [Required]
        [DisplayName("Dimension")]
        public string DimensionName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Quantity")]
        public int StorageQuantity { get; set; }

        public ICollection<ProductColorInputModel> Colors { get; set; }

        public ICollection<ProductMaterialInputModel> Materials { get; set; }
    }

    public class ProductColorInputModel : IMapFrom<ProductColor>
    {
        [DisplayName("Color")]
        public string ColorName { get; set; }
    }

    public class ProductMaterialInputModel : IMapFrom<ProductMaterial>
    {
        [DisplayName("Material")]
        public string MaterialName { get; set; }
    }
}