namespace Cosiness.Services.Data
{
    using Cosiness.Web.InputModels.Products;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<string> CreateAsync(ProductInputModel inputModel);

        Task UpdateAsync(string id, ProductInputModel inputModel);

        Task DeleteByIdAsync(string id);
    }
}