namespace Cosiness.Services.Data
{
    using System.Threading.Tasks;

    public interface ISetsService
    {
        Task<string> CreateAsync(string name);

        Task UpdateAsync(string id, string name);

        Task DeleteAsync(string id);
    }
}