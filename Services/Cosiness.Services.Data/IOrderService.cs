namespace Cosiness.Services.Data
{
    using Cosiness.Web.InputModels.Orders;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task<string> CreateAsync(OrderInputModel inputModel);

        Task UpdateStatusAsync(string id, string status);
    }
}