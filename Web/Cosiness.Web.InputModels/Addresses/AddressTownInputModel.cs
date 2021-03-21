namespace Cosiness.Web.InputModels.Addresses
{
    using Cosiness.Models;
    using Cosiness.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class AddressTownInputModel : IMapTo<Town>
    {
        [Required]
        public string Name { get; set; }
    }
}