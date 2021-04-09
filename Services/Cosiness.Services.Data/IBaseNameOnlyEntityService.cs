namespace Cosiness.Services.Data
{
    using Cosiness.Models.Common;
    using System.Threading.Tasks;

    public interface IBaseNameOnlyEntityService<TEntity>
        where TEntity : IBaseNameOnlyEntity<string>, new()
    {
        Task<string> GetIdByNameAsync(string name);

        Task DeleteAsync(string id);
    }
}