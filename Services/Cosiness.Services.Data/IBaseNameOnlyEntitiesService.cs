namespace Cosiness.Services.Data
{
    using Cosiness.Models.Common;
    using System.Threading.Tasks;

    public interface IBaseNameOnlyEntitiesService<TEntity>
        where TEntity : IBaseNameOnlyEntity<string>, new()
    {
        Task<string> GetIdByNameAsync(string name);

        Task UpdateAsync(string id, string name);

        Task DeleteAsync(string id);
    }
}