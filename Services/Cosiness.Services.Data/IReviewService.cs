
namespace Cosiness.Services.Data
{
    using Cosiness.Data;
    using Cosiness.Models;
    using Cosiness.Services.Data.Helpers;
    using Cosiness.Services.Mapping;
    using Cosiness.Web.InputModels.Reviews;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IReviewService
    {
        Task<string> CreateAsync(string creatorId, string productId, ReviewInputModel inputModel);

        Task UpdateAsync(string id, ReviewInputModel inputModel);

        Task DeleteAsync(string id);
    }

    public class ReviewService : IReviewService, IValidator
    {
        private readonly CosinessDbContext _context;

        public ReviewService(CosinessDbContext context)
        {
            this._context = context;
        }

        public async Task<string> CreateAsync(string creatorId, string productId, ReviewInputModel inputModel)
        {
            this.ThrowIfIncorrectId(_context.Products, productId);

            var review = inputModel.Map<ReviewInputModel, Review>();
            review.CreatedOn = DateTime.UtcNow;
            review.ProductId = productId;
            review.CreatorId = creatorId;

            _context.Reviews.Add(review);

            await _context.SaveChangesAsync();

            return review.Id;
        }

        public async Task UpdateAsync(string id, ReviewInputModel inputModel)
        {
            this.ThrowIfIncorrectId(_context.Reviews, id);

            var review = await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == id);

            review.Rating = inputModel.Rating;
            review.Comment = inputModel.Comment;

            _context.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            this.ThrowIfEmptyCollection(_context.Reviews);
            this.ThrowIfIncorrectId(_context.Reviews, id);

            var review = await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Reviews.Remove(review);

            await _context.SaveChangesAsync();
        }
    }
}
