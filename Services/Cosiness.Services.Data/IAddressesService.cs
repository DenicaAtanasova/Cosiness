namespace Cosiness.Services.Data
{
    using Cosiness.Web.InputModels.Addresses;
    using System.Threading.Tasks;

    public interface IAddressesService
    {
        Task<string> CreateAsync(AddressInputModel inputModel);

        Task DeleteAsync(string id);

        Task UpdateAsync(string id, AddressInputModel inputModel);

        //TODO GetByIdAsync
        //TODO GetByUserAsync
    }
}