using System.Net;
using MediatR;
using Messenger.Data;
using Messenger.Domain.Constants;
using Messenger.Domain.Entities;

namespace Messenger.BusinessLogic.Commands.Files;

public class UploadFilesHandler : IRequestHandler<UploadFilesCommand, Response<string>>
{
    private readonly DatabaseContext _context;

    public UploadFilesHandler(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<Response<string>> Handle(UploadFilesCommand request, CancellationToken cancellationToken)
    {
        long size = request.Files.Sum(x => x.Length);

        if (size > FileConstants.FileMaxSize)
        {
            return Response.Fail<string>("files too big", HttpStatusCode.BadRequest);
        }

        var filesEntities = new List<FileEntity>();

        foreach (var file in request.Files)
        {
            if (file.Length > 0)
            {
                var filePath = Path.Combine(FileConstants.StoredFilesPath, file.FileName);

                
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

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
        }
        
        _context.Files.AddRange(filesEntities);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Response.Ok<string>("Ok","files uploaded");
    }
}