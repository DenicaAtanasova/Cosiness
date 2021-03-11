namespace Cosiness.Services.Data
{
    using System.Threading.Tasks;

    public interface ICategoiesService
    {
        Task<string> CreateAsync(string name);

        Task UpdateAsync(string id, string name);

        Task DeleteAsync(string id);
    }
}