
using AutoMapper;
using Messenger.BusinessLogic.Commands.Contacts;
using Messenger.BusinessLogic.Commands.Limits;
using Messenger.BusinessLogic.Commands.Messages;
using Messenger.BusinessLogic.Commands.UserChats;
using Messenger.BusinessLogic.Queries.Messages.GetMessages;


namespace Messanger.Presentation.PresentationProfile
{
    public class PresentationProfile : Profile
    {
        public PresentationProfile()
        {
            CreateMap<SendMessageRequest,SendMessageCommand>();
            CreateMap<DeleteMessageRequest, DeleteMessageCommand>();
            CreateMap<EditMessageRequest, EditMessageCommand>();
            CreateMap<CreateChannelRequest,CreateChannelCommand>();
            CreateMap<JoinChannelRequest,JoinChannelCommand>();
            CreateMap<GetMessagesRequest, GetMessagesQuery>();
            CreateMap<AddContactRequest, AddContactCommand>();
            CreateMap<DeleteContactRequest, DeleteContactCommand>();
            CreateMap<LimitRequest, LimitUserCommand>();
        }
    }
}
