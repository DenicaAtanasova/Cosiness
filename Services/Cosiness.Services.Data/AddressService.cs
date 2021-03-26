namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data.Helpers;
    using Cosiness.Services.Mapping;
    using Cosiness.Web.InputModels.Addresses;

    using Microsoft.EntityFrameworkCore;

    using System.Threading.Tasks;

    public class AddressService : IAddressService, IValidator
    {
        private readonly CosinessDbContext _context;
        private readonly IBaseNameOnlyEntityService<Town> _townsService;

        public AddressService(
            CosinessDbContext context,
            IBaseNameOnlyEntityService<Town> townsService)
        {
            _context = context;
            _townsService = townsService;
        }

        public async Task<string> CreateAsync(AddressInputModel inputModel)
        {
            var address = inputModel.Map<AddressInputModel, Address>();

            var townId = await _townsService.GetIdByNameAsync(inputModel.Town.Name);
            address.TownId = townId;

            var createdAddress = _context.Addresses.Add(address).Entity;
            await _context.SaveChangesAsync();

            return createdAddress.Id;
        }

        public async Task DeleteAsync(string id)
        {
            this.ThrowIfEmptyCollection(_context.Addresses);

            var addressFromDb = await _context.Addresses
                .FirstOrDefaultAsync(x => x.Id == id);

            this.ThrowIfIncorrectId(addressFromDb);

            _context.Addresses.Remove(addressFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, AddressInputModel inputModel)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(x => x.Id == id);

            this.ThrowIfIncorrectId(address);
            _context.Entry(address).State = EntityState.Detached;

            address = inputModel.Map<AddressInputModel, Address>();
            address.Id = id;

            _context.Update(address);
            await _context.SaveChangesAsync();
        }       
    }
}