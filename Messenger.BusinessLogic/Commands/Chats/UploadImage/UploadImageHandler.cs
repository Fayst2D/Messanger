using System.Net;
using MediatR;
using Messenger.ApplicationServices.Interfaces;
using Messenger.Data.Database;
using Messenger.Domain.Constants;
using Messenger.Domain.Entities;
using Messenger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Commands.Chats.UploadImage;

public class UploadImageHandler : IRequestHandler<UploadImageCommand, Response<string>>
{
    private readonly DatabaseContext _context;
    private readonly IFileService _fileService;

    public UploadImageHandler(DatabaseContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    public async Task<Response<string>> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var userChatEntity = await _context.UserChats
            .Include(x => x.Chat)
            .Where(x => x.UserId == request.UserId)
            .Where(x => x.ChatId == request.ChatId)
            .Where(x => x.Chat.ChatType == (int)ChatTypes.Channel)
            .FirstOrDefaultAsync(cancellationToken);

        if (userChatEntity == null)
        {
            return Response.Fail<string>("Chat not found", HttpStatusCode.NotFound);
        }

        if (userChatEntity.RoleId < (int)UserChatRoles.Administrator)
        {
            return Response.Fail<string>("You don't have enough rights", HttpStatusCode.BadRequest);
        }

        if (_fileService.IsTooBig(request.Image, FileConstants.FileMaxSize))
        {
            return Response.Fail<string>("Image is too big", HttpStatusCode.BadRequest);
        }

        if (!_fileService.IsImage(request.Image))
        {
            return Response.Fail<string>("Chosen file isn't image", HttpStatusCode.BadRequest);
        }

        var filePath = _fileService.GenerateUniquePath(FileConstants.StoredFilesPath, request.Image.FileName);
        await _fileService.UploadFile(request.Image,filePath,cancellationToken);

        var fileEntity = new FileEntity
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ContentType = request.Image.ContentType,
            FilePath = filePath,
            UploadedAt = DateTime.Now
        };
        
        _context.Files.Add(fileEntity);
        
        userChatEntity.Chat.Image  = filePath;
        _context.Chats.Update(userChatEntity.Chat);

        await _context.SaveChangesAsync(cancellationToken);
        
        
        return Response.Ok<string>("Ok", "Chat image changed");
    }
}