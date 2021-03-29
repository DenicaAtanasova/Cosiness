namespace Cosiness.Services.Data
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IImageService
    {
        Task<string> CreateAsync(string productId, string fileName, Stream fileContent);

        Task UpdateAsync(string id, string fileName, Stream fileContent);

        Task DeleteAsync(string id);
    }
}