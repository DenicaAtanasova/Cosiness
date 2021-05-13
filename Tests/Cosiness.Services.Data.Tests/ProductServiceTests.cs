namespace Cosiness.Services.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Models.Common;
    using Cosiness.Services.Data.Tests.Common;
    using Cosiness.Web.InputModels.Products;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class ProductServiceTests
    {
        private readonly CosinessDbContext _context;
        private readonly IProductService _productService;

        private readonly string _categorySheetsId;
        private readonly string _categorySheetsName = "Sheets";
        private readonly string _categoryPillowId;
        private readonly string _categoryPillowName = "Pillow";

        private readonly string _setMoonId;
        private readonly string _setMoonName = "Moon";
        private readonly string _setSpaceId;
        private readonly string _setSpaceName = "Space";

        private readonly string _dimensionLargeId;
        private readonly string _dimensionLargeName = "200/160";
        private readonly string _dimensionSmallId;
        private readonly string _dimensionSmallName = "70/50";

        private readonly string _colorBlueId;
        private readonly string _colorBlueName = "Blue";
        private readonly string _colorRedId;
        private readonly string _colorRedName = "Red";

        private readonly string _materialSilkId;
        private readonly string _materialSilkName = "Silk";
        private readonly string _materialSatinId;
        private readonly string _materialSatinName = "Satin";

        private readonly string _imageName = "image.png";
        private readonly IFormFile _image;

        private string _productId;
        public ProductServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CosinessDbContext(options);

            _categorySheetsId = SetBaseNameOnlyEntityId<Category>(_categorySheetsName);
            _categoryPillowId = SetBaseNameOnlyEntityId<Category>(_categoryPillowName);

            _setMoonId = SetBaseNameOnlyEntityId<Set>(_setMoonName);
            _setSpaceId = SetBaseNameOnlyEntityId<Set>(_setSpaceName);

            _dimensionLargeId = SetBaseNameOnlyEntityId<Dimension>(_dimensionLargeName);
            _dimensionSmallId = SetBaseNameOnlyEntityId<Dimension>(_dimensionSmallName);

            _colorBlueId = SetBaseNameOnlyEntityId<Color>(_colorBlueName);
            _colorRedId = SetBaseNameOnlyEntityId<Color>(_colorRedName);

            _materialSilkId = SetBaseNameOnlyEntityId<Material>(_materialSilkName);
            _materialSatinId = SetBaseNameOnlyEntityId<Material>(_materialSatinName);

            this.SeedData();

            _context.SaveChanges();

            var categoryService = new Mock<IBaseNameOnlyEntityService<Category>>();
            categoryService.Setup(x => x.GetIdByNameAsync(_categorySheetsName))
                .ReturnsAsync(_categorySheetsId);
            categoryService.Setup(x => x.GetIdByNameAsync(_categoryPillowName))
                .ReturnsAsync(_categoryPillowId);

            var setService = new Mock<IBaseNameOnlyEntityService<Set>>();
            setService.Setup(x => x.GetIdByNameAsync(_setMoonName))
                .ReturnsAsync(_setMoonId);
            setService.Setup(x => x.GetIdByNameAsync(_setSpaceName))
                .ReturnsAsync(_setSpaceId);

            var dimensionService = new Mock<IBaseNameOnlyEntityService<Dimension>>();
            dimensionService.Setup(x => x.GetIdByNameAsync(_dimensionLargeName))
                .ReturnsAsync(_dimensionLargeId);
            dimensionService.Setup(x => x.GetIdByNameAsync(_dimensionSmallName))
                .ReturnsAsync(_dimensionSmallId);

            var colorService = new Mock<IBaseNameOnlyEntityService<Color>>();
            colorService.Setup(x => x.GetIdByNameAsync(_colorBlueName))
                .ReturnsAsync(_colorBlueId);
            colorService.Setup(x => x.GetIdByNameAsync(_colorRedName))
                .ReturnsAsync(_colorRedId);

            var materialService = new Mock<IBaseNameOnlyEntityService<Material>>();
            materialService.Setup(x => x.GetIdByNameAsync(_materialSilkName))
                .ReturnsAsync(_materialSilkId);
            materialService.Setup(x => x.GetIdByNameAsync(_materialSatinName))
                .ReturnsAsync(_materialSatinId);

            var imageService = new Mock<IImageService>();

            var imageMock = new Mock<IFormFile>();
            imageMock.Setup(x => x.FileName)
                .Returns(_imageName);
            imageMock.Setup(x => x.OpenReadStream())
                .Returns(new MemoryStream());
            _image = imageMock.Object;

            _productService = new ProductService(
                _context,
                categoryService.Object,
                setService.Object,
                dimensionService.Object,
                colorService.Object,
                materialService.Object,
                imageService.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var product = new ProductInputModel
            {
                Category = _categoryPillowName,
                Set = _setMoonName,
                Dimension = _dimensionSmallName,
                Colors = new List<string> { _colorBlueName, _colorRedName},
                Materials = new List<string> { _materialSatinName, _materialSilkName },
                Image = _image,
                Price = 200M,
                StorageQuantity = 28
            };

            var productId = await _productService.CreateAsync(product);

            var productFromDb = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == productId);

            Assert.Equal(product.Category, productFromDb.Category.Name);
            Assert.Equal(product.Set, productFromDb.Set.Name);
            Assert.Equal(product.Dimension, productFromDb.Dimension.Name);
            Assert.Equal(product.StorageQuantity, productFromDb.Storage.Quantity);
            Assert.Equal(product.Price, productFromDb.Price);
            Assert.Equal(product.Colors, productFromDb.Colors.Select(x => x.Color.Name));
            Assert.Equal(product.Materials, productFromDb.Materials.Select(x => x.Material.Name));
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldWorkCorrectly()
        {
            Assert.NotEmpty(_context.Products);

            await _productService.DeleteByIdAsync(_productId);

            Assert.Empty(_context.Addresses);

            var productReviews = await _context.Reviews
                .Where(x => x.ProductId == _productId)
                .CountAsync();

            Assert.Equal(0, productReviews);

            var productImage = await _context.Images
                .Where(x => x.ProductId == _productId)
                .CountAsync();
            Assert.Equal(0, productImage);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldThrowWhenEmptyCollection()
        {
            await _productService.DeleteByIdAsync(_productId);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _productService.DeleteByIdAsync(_productId));
            var epectedMessage = ErrorMessage.GetEmptyCollectionMessage(_productService.GetType().Name);

            Assert.Equal(epectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _productService.DeleteByIdAsync(incorrectId));
            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_productService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldWorkCorrectly()
        {
            var product = new ProductInputModel
            {
                Category = _categoryPillowName,
                Set = _setMoonName,
                Dimension = _dimensionSmallName,
                Colors = new List<string> { _colorBlueName, _colorRedName },
                Materials = new List<string> { _materialSatinName, _materialSilkName },
                Image = _image
            };

            await _productService.UpdateAsync(_productId, product);

            var productFromDb = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == _productId);

            Assert.Equal(product.Category, productFromDb.Category.Name);
            Assert.Equal(product.Set, productFromDb.Set.Name);
            Assert.Equal(product.Dimension, productFromDb.Dimension.Name);
            Assert.Equal(product.Colors, productFromDb.Colors.Select(x => x.Color.Name));
            Assert.Equal(product.Materials, productFromDb.Materials.Select(x => x.Material.Name));
            Assert.Equal(product.Image.FileName, productFromDb.Image.Caption);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowWhenIncorrectId()
        {
            var incorrectId = Guid.NewGuid().ToString();
            var product = new ProductInputModel
            {
                Category = _categoryPillowName,
                Set = _setMoonName,
                Dimension = _dimensionSmallName,
                Colors = new List<string> { _colorBlueName, _colorRedName },
                Materials = new List<string> { _materialSatinName, _materialSilkName },
                Image = _image
            };

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _productService.UpdateAsync(incorrectId, product));

            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_productService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        private string SetBaseNameOnlyEntityId<TEntity>(string name)
            where TEntity : BaseNameOnlyEntity<string>, new()
        {
            var entity = new TEntity
            {
                Name = name
            };

            _context.Set<TEntity>().Add(entity);

            return entity.Id;
        }

        private void SeedData()
        {
            var product = _context.Products.Add(
                new Product
                {
                    CategoryId = _categorySheetsId,
                    SetId = _setSpaceId,
                    DimensionId = _dimensionLargeId,
                    Image = new Image
                    {
                        Caption = _imageName,
                        Url = "image-url"
                    },
                    Storage = new Storage { Quantity = 10}
                }).Entity;

            _productId = product.Id;
        }
    }
}