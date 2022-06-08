using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Messenger.Data;
using Messenger.BusinessLogic.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.BusinessLogic.Queries.Messages.GetMessages
{
    public class GetMessagesRequest
    {
        public Guid ChatId { get; set; }
    }
    public class GetMessagesQuery : BaseRequest, IRequest<Response<IEnumerable<Message>>>
    {
        public Guid ChatId { get; set; }
    }

    public class GetMessagesHandler : IRequestHandler<GetMessagesQuery, Response<IEnumerable<Message>>>
    {
        private readonly DatabaseContext _context;

        public GetMessagesHandler(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Response<IEnumerable<Message>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _context
                .Messages
                .AsNoTracking()
                .Where(x => x.ChatId == request.ChatId)
                .OrderBy(x => x.CreatedAt)
                .Select(messageEntity => new Message
                {
                    Id = messageEntity.Id,
                    ChatId = messageEntity.ChatId,
                    UserId = messageEntity.UserId,
                    MessageText = messageEntity.MessageText,
                    CreatedAt = messageEntity.CreatedAt.ToShortTimeString()
                }).Take(100).ToListAsync();

            return Response.Ok<IEnumerable<Message>>("Ok",messages);
        }
    }
}
