namespace Cosiness.Services.Data
{
    using System.Threading.Tasks;

    public interface IShoppingCartService
    {
        Task ClearAsync(string id);
    }
}