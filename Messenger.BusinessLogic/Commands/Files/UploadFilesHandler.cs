using System.Net;
using MediatR;
using Messenger.ApplicationServices.Interfaces;
using Messenger.Data.Database;
using Messenger.Domain.Constants;
using Messenger.Domain.Entities;

namespace Messenger.BusinessLogic.Commands.Files;

public class UploadFilesHandler : IRequestHandler<UploadFilesCommand, Response<IEnumerable<string>>>
{
    private readonly DatabaseContext _context;
    private readonly IFileService _fileService;

    public UploadFilesHandler(DatabaseContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }
    
    public async Task<Response<IEnumerable<string>>> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
    {

        if (_fileService.IsTooBig(request.Files, FileConstants.FileMaxSize))
        {
            return Response.Fail<IEnumerable<string>>("Files are too big", HttpStatusCode.BadRequest);
        }
        
        var filesEntities = new List<FileEntity>();
        var filePaths = new List<string>();

        foreach (var file in request.Files)
        {
            var filePath = _fileService.GenerateUniquePath(FileConstants.StoredFilesPath) + file.FileName;
            filePaths.Add(filePath);

            await _fileService.UploadFile(file, filePath, cancellationToken);
            
            var fileEntity = new FileEntity
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ContentType = file.ContentType,
                FilePath = filePath,
                UploadedAt = DateTime.Now
            };

            filesEntities.Add(fileEntity);
        }
        
        _context.Files.AddRange(filesEntities);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Response.Ok<IEnumerable<string>>("Ok",filePaths);
    }
}