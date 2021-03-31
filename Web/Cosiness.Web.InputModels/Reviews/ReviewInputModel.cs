namespace Cosiness.Web.InputModels.Reviews
{
    using Cosiness.Models;
    using Cosiness.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class ReviewInputModel : IMapTo<Review>
    {
        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}