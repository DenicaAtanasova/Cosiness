namespace Cosiness.Services.Storage
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IFileStorage
    {
        Task<string> UploadAsync(string FileName, Stream content);

        Task DeleteAsync(string filenName);
    }
}