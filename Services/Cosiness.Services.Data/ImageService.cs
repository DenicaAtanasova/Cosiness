namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data.Helpers;
    using Cosiness.Services.Storage;

    using Microsoft.EntityFrameworkCore;

    using System.IO;
    using System.Threading.Tasks;

    public class ImageService : IImageService, IValidator
    {
        private readonly CosinessDbContext _context;
        private readonly IFileStorage _blobStorageService;

        public ImageService(
            CosinessDbContext context,
            IFileStorage blobStorageService)
        {
            this._context = context;
            this._blobStorageService = blobStorageService;
        }

        public async Task<string> CreateAsync(string productId, string fileName, Stream fileContent)
        {
            this.ThrowIfNullOrEmpty(productId);

            var url = await _blobStorageService
                .UploadAsync(fileName, fileContent);

            var image = new Image
            {
                Caption = fileName,
                Url = url,
                ProductId = productId
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return image.Id;
        }

        public async Task UpdateAsync(string id, string fileName, Stream fileContent)
        {
            this.ThrowIfIncorrectId(_context.Images, id);

            var image = await _context.Images
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Entry(image).State = EntityState.Detached;

            await _blobStorageService.DeleteAsync(image.Caption);
            image.Url = await _blobStorageService.UploadAsync(fileName, fileContent);
            image.Caption = fileName;

            _context.Images.Update(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            this.ThrowIfEmptyCollection(_context.Images);
            this.ThrowIfIncorrectId(_context.Images, id);

            var imageFromDb = await _context.Images
                .FirstOrDefaultAsync(x => x.Id == id);

            await _blobStorageService.DeleteAsync(imageFromDb.Caption);
            _context.Remove(imageFromDb);

            await _context.SaveChangesAsync();
        }
    }
}