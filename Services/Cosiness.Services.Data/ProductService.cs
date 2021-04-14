namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data.Helpers;
    using Cosiness.Web.InputModels.Products;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductService : IProductService, IValidator
    {
        private readonly CosinessDbContext _context;
        private readonly IBaseNameOnlyEntityService<Category> _categoryService;
        private readonly IBaseNameOnlyEntityService<Set> _setService;
        private readonly IBaseNameOnlyEntityService<Dimension> _dimensionService;
        private readonly IBaseNameOnlyEntityService<Color> _colorService;
        private readonly IBaseNameOnlyEntityService<Material> _materialService;
        private readonly IImageService _imageService;

        public ProductService(
            CosinessDbContext context,
            IBaseNameOnlyEntityService<Category> categoryService,
            IBaseNameOnlyEntityService<Set> setService,
            IBaseNameOnlyEntityService<Dimension> dimensionService,
            IBaseNameOnlyEntityService<Color> colorService,
            IBaseNameOnlyEntityService<Material> materialService,
            IImageService imageService)
        {
            _context = context;
            _categoryService = categoryService;
            _setService = setService;
            _dimensionService = dimensionService;
            _colorService = colorService;
            _materialService = materialService;
            _imageService = imageService;
        }

        public async Task<string> CreateAsync(ProductInputModel inputModel)
        {
            var product = new Product
            {
                CreatedOn = DateTime.UtcNow,
                RefNumber = inputModel.RefNumber,
                Price = inputModel.Price,
                CategoryId = await _categoryService.GetIdByNameAsync(inputModel.Category),
                SetId = await _setService.GetIdByNameAsync(inputModel.Set),
                DimensionId = await _dimensionService.GetIdByNameAsync(inputModel.Dimension)
            };

            _context.Products.Add(product);
            await AddColorsToProductAsync(product.Id, inputModel.Colors);
            await AddMaterialsToProductAsync(product.Id, inputModel.Materials);

            await _context.SaveChangesAsync();

            await _imageService.CreateAsync(product.Id, inputModel.Image);

            return product.Id;
        }

        public async Task DeleteByIdAsync(string id)
        {
            this.ThrowIfEmptyCollection(_context.Products);
            this.ThrowIfIncorrectId(_context.Products, id);

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);

            await _imageService.DeleteAsync(product.Image.Id);
            _context.Products.Remove(product);

            this.DeleteReviews(product.Id);

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, ProductInputModel inputModel)
        {
            this.ThrowIfIncorrectId(_context.Products, id);

            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == id);

            product.Price = inputModel.Price;
            product.CategoryId = await _categoryService.GetIdByNameAsync(inputModel.Category);
            product.SetId = await _setService.GetIdByNameAsync(inputModel.Set);
            product.DimensionId = await _dimensionService.GetIdByNameAsync(inputModel.Dimension);
            await AddColorsToProductAsync(product.Id, inputModel.Colors);
            await AddMaterialsToProductAsync(product.Id, inputModel.Materials);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            await _imageService.UpdateAsync(product.Id, inputModel.Image);
        }

        private async Task AddColorsToProductAsync (string productId, IEnumerable<string> colors)
        {
            var productColors = new List<ProductColor>();
            foreach (var colorName in colors)
            {
                var colorId = await _colorService.GetIdByNameAsync(colorName);

                var productColor = new ProductColor
                {
                    ColorId = colorId,
                    ProductId = productId
                };
                productColors.Add(productColor);
            }

            _context.ProductsColors.AddRange(productColors);
        }

        private async Task AddMaterialsToProductAsync(string productId, IEnumerable<string> materials)
        {
            var productMaterials = new List<ProductMaterial>();
            foreach (var materialName in materials)
            {
                var materialId = await _materialService.GetIdByNameAsync(materialName);

                var productMaterial = new ProductMaterial
                {
                    MaterialId = materialId,
                    ProductId = productId
                };
                productMaterials.Add(productMaterial);
            }

            _context.ProductsMaterials.AddRange(productMaterials);
        }

        private void DeleteReviews(string productId)
        {
            var reviwesToDelete = _context.Reviews
                .Where(x => x.ProductId == productId);

            _context.Reviews.RemoveRange(reviwesToDelete);
        }
    }
}