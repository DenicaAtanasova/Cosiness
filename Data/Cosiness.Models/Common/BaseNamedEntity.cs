namespace Cosiness.Models.Common
{
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class BaseNamedEntity<TKey>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        public string Name { get; set; }
    }
}