using VideoUploadingTest.Models;

namespace VideoUploadingTest.Services.Interface
{
    public interface IFileService
    {
        IEnumerable<FileModel> GetFiles();
        Task<bool> UploadFile(IFormFile file);
    }
}
