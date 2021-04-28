namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models;

    using System.Threading.Tasks;

    public class StorageService : IStorageService
    {
        private readonly CosinessDbContext _context;

        public StorageService(CosinessDbContext context)
        {
            this._context = context;
        }

        public async Task<string> CreateAsync(string productId, int quantity)
        {
            var storage = new Storage
            {
                ProductId = productId,
                Quantity = quantity
            };

            _context.Storages.Add(storage);
            await _context.SaveChangesAsync();

            return storage.Id;
        }
    }
}
