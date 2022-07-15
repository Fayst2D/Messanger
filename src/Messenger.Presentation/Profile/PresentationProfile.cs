using Messenger.BusinessLogic.Commands.Chats.CreateChannel;
using Messenger.BusinessLogic.Commands.Limits.RemoveUserLimit;
using Messenger.BusinessLogic.Commands.Messages.Delete;
using Messenger.BusinessLogic.Commands.Messages.Edit;
using Messenger.BusinessLogic.Commands.Messages.Send;

namespace Messenger.Presentation.Profile;

public class PresentationProfile : AutoMapper.Profile
{
    public PresentationProfile()
    {
        CreateMap<SendMessageRequest,SendMessageCommand>();
        CreateMap<DeleteMessageRequest, DeleteMessageCommand>();
        CreateMap<EditMessageRequest, EditMessageCommand>();
        CreateMap<CreateChannelRequest,CreateChannelCommand>();
        CreateMap<RemoveUserLimitRequest, RemoveUserLimitCommand>();
    }
}