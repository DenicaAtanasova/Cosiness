namespace Cosiness.Models.Common
{
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }
    }
}