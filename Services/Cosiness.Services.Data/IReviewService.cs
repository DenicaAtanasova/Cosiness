
namespace Cosiness.Services.Data
{
    using Cosiness.Web.InputModels.Reviews;
    using System.Threading.Tasks;

    public interface IReviewService
    {
        Task<string> CreateAsync(string creatorId, string productId, ReviewInputModel inputModel);

        Task UpdateAsync(string id, ReviewInputModel inputModel);

        Task DeleteAsync(string id);
    }
}
