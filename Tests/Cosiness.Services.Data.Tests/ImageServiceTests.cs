namespace Cosiness.Services.Data.Tests
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data;
    using Cosiness.Services.Data.Tests.Common;
    using Cosiness.Services.Storage;
    using Microsoft.AspNetCore.Http;
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

        private readonly Stream _minnieFileContent;
        private readonly Stream _spaceFileContent;

        private readonly IFormFile _minnieImageFile;
        private readonly IFormFile _spaceImageFile;

        public ImageServiceTests()
        {
            var options = new DbContextOptionsBuilder<CosinessDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new CosinessDbContext(options);

            SeedData();

            _minnieFileContent = new MemoryStream();
            var minnieImageMock = new Mock<IFormFile>();
            minnieImageMock.Setup(x => x.FileName)
                .Returns(_minnieImage);
            minnieImageMock.Setup(x => x.OpenReadStream())
                .Returns(_minnieFileContent);
            _minnieImageFile = minnieImageMock.Object;

            _spaceFileContent = new MemoryStream();
            var spaceImageMock = new Mock<IFormFile>();
            spaceImageMock.Setup(x => x.FileName)
                .Returns(_spaceImage);
            spaceImageMock.Setup(x => x.OpenReadStream())
                .Returns(_spaceFileContent);
            _spaceImageFile = spaceImageMock.Object;

            var storageService = new Mock<IFileStorage>();
            storageService.Setup(x => x.UploadAsync(_minnieImage, _minnieFileContent))
                .ReturnsAsync(_minnieUrl);
            storageService.Setup(x => x.UploadAsync(_spaceImage, _spaceFileContent))
                .ReturnsAsync(_spaceUrl);

            _imageService = new ImageService(_context, storageService.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var createdImageId = await _imageService
                .CreateAsync(_productId, _minnieImageFile);

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
                async () => await _imageService.CreateAsync(productId, _minnieImageFile));

            var expectedMessage = ErrorMessage.GetNullOrEmptyParameterMessage(_imageService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_ShouldWorkCorrectly()
        {
            await _imageService.UpdateAsync(_imageId, _spaceImageFile);

            var image = await _context.Images
                .FirstOrDefaultAsync(x => x.Id == _imageId);

            Assert.Equal(image.Caption, _spaceImage);
            Assert.Equal(image.Url, _spaceUrl);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowWhenIncorrectId()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _imageService.UpdateAsync(Guid.NewGuid().ToString(), _minnieImageFile));

            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_imageService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldWorkCorrectly()
        {
            Assert.NotEmpty(_context.Images);

            await _imageService.DeleteAsync(_imageId);

            Assert.Empty(_context.Images);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenEmpryCollection()
        {
            await _imageService.DeleteAsync(_imageId);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _imageService.DeleteAsync(_imageId));

            var expectedMessage = ErrorMessage.GetEmptyCollectionMessage(_imageService.GetType().Name);

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowWhenIncorrectId()
        {
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _imageService.DeleteAsync(Guid.NewGuid().ToString()));

            var expectedMessage = ErrorMessage.GetIncorrectIdMessage(_imageService.GetType().Name);

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