namespace Cosiness.Services.Data
{
    using Microsoft.AspNetCore.Http;

    using System.Threading.Tasks;

    public interface IImageService
    {
        Task<string> CreateAsync(string productId, IFormFile imageFile);

        Task UpdateAsync(string id, IFormFile imageFile);

        Task DeleteAsync(string id);
    }
}