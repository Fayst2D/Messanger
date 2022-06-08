using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Messenger.Data;
using Messenger.Data.Migrations;
using Messenger.Domain.Entities;
using Messenger.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Messenger.BusinessLogic.Commands.Messages
{
    public class SendMessageRequest
    {
        public string Message { get; set; }
        public Guid ChatId { get; set; }
    }

    public class SendMessageCommand : BaseRequest, IRequest<Response<Message>>
    {
        public string Message { get; set; }
        public Guid ChatId { get; set; }
    }
    
    public class SendMessageHandler : IRequestHandler<SendMessageCommand, Response<Message>>
    {
        private readonly DatabaseContext _context;
        public SendMessageHandler(DatabaseContext context)
        {
            _context = context;       
        }

        public async Task<Response<Message>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var messageEntity = new MessageEntity
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ChatId = request.ChatId,
                CreatedAt = DateTime.Now,
                MessageText = request.Message
            };

            _context.Messages.Add(messageEntity);
            await _context.SaveChangesAsync();

            var message = new Message
            {
                Id = messageEntity.Id,
                UserId = messageEntity.UserId,
                ChatId = messageEntity.ChatId,
                CreatedAt = messageEntity.CreatedAt.ToString(),
                MessageText = messageEntity.MessageText
            };

            return Response.Ok("Ok", message);
        }
    }
}
