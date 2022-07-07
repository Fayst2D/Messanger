using System.Globalization;
using System.Net.Mime;
using Messenger.ApplicationServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Messenger.ApplicationServices.Services;

public class FileService : IFileService
{
    public bool IsTooBig(ICollection<IFormFile> files, long fileMaxSize)
    {
        long size = files.Sum(x => x.Length);

        if (size > fileMaxSize)
        {
            return true;
        }

        return false;
    }

    public bool IsTooBig(IFormFile file, long fileMaxSize)
    {
        long size = file.Length;

        if (size > fileMaxSize)
        {
            return true;
        }

        return false;
    }

    private string GetFileType(string contentType)
    {
        string fileType = contentType;

        fileType = fileType.Remove(fileType.IndexOf('/'));

        return fileType;
    }

    public bool IsImage(IFormFile file)
    {
        if (GetFileType(file.ContentType) == "image")
        {
            return true;
        }

        return false;
    }

    public string GenerateUniquePath(string basePath)
    {
        string uniquePath = $"{basePath+DateTime.Now.ToShortDateString()+Guid.NewGuid().ToString()}";

        return uniquePath;
    }

    public async Task UploadFile(IFormFile file, string path, CancellationToken cancellationToken)
    {
        if (file.Length > 0)
        {
            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }
        }
        
    }
}