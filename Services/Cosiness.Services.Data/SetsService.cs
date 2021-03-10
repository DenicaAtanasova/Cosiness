namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SetsService : ISetsService
    {
        private readonly CosinessDbContext _context;

        public SetsService(CosinessDbContext context)
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