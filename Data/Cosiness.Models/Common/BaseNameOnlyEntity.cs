namespace Cosiness.Models.Common
{
    public abstract class BaseNameOnlyEntity<TKey> 
        : BaseEntity<TKey>, IBaseNameOnlyEntity<TKey>
    {
        public string Name { get; set; }
    }
}