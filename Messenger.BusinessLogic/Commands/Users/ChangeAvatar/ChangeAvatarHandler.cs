using System.Net;
using MediatR;
using Messenger.ApplicationServices.Interfaces;
using Messenger.Data.Database;
using Messenger.Domain.Constants;
using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Users.ChangeAvatar;

public class ChangeAvatarHandler : IRequestHandler<ChangeAvatarCommand, Response<string>>
{
    private readonly DatabaseContext _context;
    private readonly IFileService _fileService;

    public ChangeAvatarHandler(DatabaseContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Response<string>> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        var userEntity = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        
        if(_fileService.IsTooBig(request.Avatar,FileConstants.FileMaxSize))
        {
            return Response.Fail<string>("Image is too big", HttpStatusCode.BadRequest);
        }
        
        if (!_fileService.IsImage(request.Avatar))
        {
            return Response.Fail<string>("Chosen file isn't image", HttpStatusCode.BadRequest);
        }
        
        var filePath = _fileService.GenerateUniquePath(FileConstants.StoredFilesPath, request.Avatar.FileName);
        await _fileService.UploadFile(request.Avatar,filePath,cancellationToken);
        
        var fileEntity = new FileEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ContentType = request.Avatar.ContentType,
            FilePath = filePath,
            UploadedAt = DateTime.Now
        };
        
        
        _context.Files.Add(fileEntity);

        userEntity.Avatar = filePath;
        _context.Users.Update(userEntity);

        await _context.SaveChangesAsync(cancellationToken);
        
        return Response.Ok<string>("Ok","User image changed");
    }
}