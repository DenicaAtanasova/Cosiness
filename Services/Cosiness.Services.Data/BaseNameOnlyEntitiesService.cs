namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models.Common;
    using Cosiness.Services.Data.Helpers;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;

    public class BaseNameOnlyEntitiesService<TEntity> 
        : IBaseNameOnlyEntitiesService<TEntity>, IValidator
        where TEntity : BaseNameOnlyEntity<string>, new()
    {
        private readonly CosinessDbContext _context;
        public BaseNameOnlyEntitiesService(CosinessDbContext context)
        {
            _context = context;
        }
         
        public async Task<string> GetIdByNameAsync(string name)
        {
            this.ThrowIfNullOrEmpty(name);

            var entityFromDb = await _context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Name == name);

            if (entityFromDb is not null)
            {
                return entityFromDb.Id;
            }

            return await this.CreateAsync(name);
        }

        public async Task DeleteAsync(string id)
        {
            this.ThrowIfEmptyCollection(_context.Set<TEntity>());

            var entityFromDb = await _context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id);

            this.ThrowIfIncorrectId(entityFromDb);

            _context.Set<TEntity>().Remove(entityFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, string name)
        {
            this.ThrowIfNullOrEmpty(name);

            var entityFromDb = await _context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id);
            this.ThrowIfIncorrectId(entityFromDb);

            entityFromDb.Name = name;

            _context.Set<TEntity>().Update(entityFromDb);
            await _context.SaveChangesAsync();
        }

        private async Task<string> CreateAsync(string name)
        {
            var entity = new TEntity
            {
                Name = name
            };

            var createdEntity = _context.Set<TEntity>().Add(entity).Entity;
            await _context.SaveChangesAsync();

            return createdEntity.Id;
        }
    }
}