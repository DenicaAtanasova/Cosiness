namespace Cosiness.Models.Common
{
    public interface IBaseNameOnlyEntity<TKey>
    {
        TKey Id { get; set; }

        string Name { get; set; }
    }
}