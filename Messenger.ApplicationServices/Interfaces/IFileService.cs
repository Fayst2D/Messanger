using Microsoft.AspNetCore.Http;

namespace Messenger.ApplicationServices.Interfaces;

public interface IFileService
{  
    public bool IsTooBig(ICollection<IFormFile> files, long fileMaxSize);
    public bool IsTooBig(IFormFile file, long fileMaxSize);
    public bool IsImage(IFormFile file);
    public string GenerateUniquePath(string basePath, string fileName);
    public Task UploadFile(IFormFile file, string path, CancellationToken cancellationToken);
}