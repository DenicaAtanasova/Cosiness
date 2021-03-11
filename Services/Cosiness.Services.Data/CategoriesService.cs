namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Services.Data.Helpers;

    using System;
    using System.Threading.Tasks;

    public class CategoriesService : ICategoiesService, IValidator
    {
        private readonly CosinessDbContext _context;

        public CategoriesService(CosinessDbContext context)
        {
            _context = context;
        }

        public Task<string> CreateAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string id, string name)
        {
            throw new NotImplementedException();
        }
    }
}