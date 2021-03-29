namespace Cosiness.Services.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data;
    using Cosiness.Services.Data.Tests.Common;
    using Cosiness.Services.Storage;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Xunit;

    public class ImageServiceTests
    {
        private readonly CosinessDbContext _context;
        private readonly IImageService _imageService;

        private string _productId;
        private string _imageId;

        private readonly string _minnieImage = "Minnie-mouse-pillow.jpg";
        private readonly string _spaceImage = "Space-pillow.jpg";

        private readonly string _minnieUrl = "minnie-mouse-url";
        private readonly string _spaceUrl = "space-pillow-url";

        private readonly Stream _fileContent;

        public ImageServiceTests()
        {
            _fileContent = new MemoryStream();
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CosinessDbContext(options);

            SeedData();

            var storageService = new Mock<IFileStorage>();
            storageService.Setup(x => x.UploadAsync(_minnieImage, _fileContent))
                .ReturnsAsync(_minnieUrl);
            storageService.Setup(x => x.UploadAsync(_spaceImage, _fileContent))
                .ReturnsAsync(_spaceUrl);

            _imageService = new ImageService(_context, storageService.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var createdImageId = await _imageService
                .CreateAsync(_productId, _minnieImage, _fileContent);

            var imageFromDb = await _context.Images
                .FirstOrDefaultAsync(x => x.Id == createdImageId);

            Assert.Equal(_minnieImage, imageFromDb.Caption);
            Assert.Equal(_minnieUrl, imageFromDb.Url);
        }

        [InlineData(null)]
        [InlineData("")]
        [Theory]
        public async Task CreateAsync_ShouldThrowWhenProductIdNullOrEmpty(string productId)
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _imageService.CreateAsync(productId, _minnieImage, _fileContent));

            var expectedMessage = ErrorMessage.GetNullOrEmptyParameterMessage(_imageService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        private void SeedData()
        {
            var product = _context.Products.Add(
                new Product
                {
                    CreatedOn = DateTime.UtcNow
                }).Entity;

            _productId = product.Id;

            var image = _context.Images.Add(
                new Image
                {
                    Product = product,
                    Caption = _minnieImage,
                    Url = _minnieUrl
                }).Entity;

            _imageId = image.Id;

            _context.SaveChanges();
        }
    }
}