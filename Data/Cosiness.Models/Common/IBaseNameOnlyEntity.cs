namespace Cosiness.Models.Common
{
    public interface IBaseNameOnlyEntity<TKey>
    {
        string Name { get; set; }
    }
}