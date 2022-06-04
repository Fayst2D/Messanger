using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Messanger.BusinessLogic.Models;
using Messanger.DataAccess;
using Messanger.DataAccess.Migrations;
using Messanger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Messanger.BusinessLogic.Commands.Messages
{
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
