using VideoUploadingTest.Models;
using VideoUploadingTest.Services.Interface;

namespace VideoUploadingTest.Services
{
    public class FileService : IFileService
    {
        private readonly string _mediaFolder;

        public FileService(string mediaFolder)
        {
            _mediaFolder = mediaFolder;
        }
        public IEnumerable<FileModel> GetFiles()
        {
            return Directory.GetFiles(_mediaFolder)
                            .Select(file => new FileModel
                            {
                                Name = Path.GetFileName(file),
                                Size = new FileInfo(file).Length
                            })
                            .ToList();
        }
        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            bool returnValue = false;
            try
            {
                    if (file.Length > 0)
                    {
                        path = _mediaFolder;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        returnValue = true;
                    }
                    else
                    {
                        returnValue = false;
                    }
            }
            catch (Exception)
            {
                returnValue = false;
            }
            return returnValue;
        }
    }
}
