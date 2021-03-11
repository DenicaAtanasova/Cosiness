namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data.Helpers;

    using Microsoft.EntityFrameworkCore;

    using System.Threading.Tasks;

    public class SetsService : ISetsService, IValidator
    {
        private readonly CosinessDbContext _context;

        public SetsService(CosinessDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAsync(string name)
        {
            this.ThrowIfNullOrEmpty(name);

            var setFromDb = await _context.Sets
                .FirstOrDefaultAsync(x => x.Name == name);
            if (setFromDb is not null)
            {
                return setFromDb.Id;
            }

            var set = new Set
            {
                Name = name
            };

            var createdEntity = _context.Sets.Add(set).Entity;
            await _context.SaveChangesAsync();

            return createdEntity.Id;
        }

        public async Task DeleteAsync(string id)
        {
            this.ThrowIfEmptyCollection(_context.Sets);

            var setFromDb = await _context.Sets
                .FirstOrDefaultAsync(x => x.Id == id);

            this.ThrowIfIncorrectId(setFromDb, id);

            _context.Sets.Remove(setFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, string name)
        {
            this.ThrowIfNullOrEmpty(name);

            var setFromDb = await _context.Sets
                .FirstOrDefaultAsync(x => x.Id == id);
            this.ThrowIfIncorrectId(setFromDb, id);

            setFromDb.Name = name;

            _context.Sets.Update(setFromDb);
            await _context.SaveChangesAsync();
        }
    }
}