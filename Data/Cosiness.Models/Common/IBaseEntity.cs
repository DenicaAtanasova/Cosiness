namespace Cosiness.Models.Common
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
    }
}