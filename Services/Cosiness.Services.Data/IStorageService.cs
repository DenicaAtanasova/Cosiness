namespace Cosiness.Services.Data
{
    using System.Threading.Tasks;

    public interface IStorageService
    {
        Task<string> CreateAsync(string productId, int quantity);
    }
}