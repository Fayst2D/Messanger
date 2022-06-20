using AutoMapper;
using Messenger.BusinessLogic.Commands.Contacts.Add;
using Messenger.BusinessLogic.Commands.Contacts.Delete;
using Messenger.BusinessLogic.Commands.Limits.LimitUser;
using Messenger.BusinessLogic.Commands.Messages.Delete;
using Messenger.BusinessLogic.Commands.Messages.Edit;
using Messenger.BusinessLogic.Commands.Messages.Send;
using Messenger.BusinessLogic.Commands.UserChats.CreateChannel;
using Messenger.BusinessLogic.Commands.UserChats.JoinChannel;
using Messenger.BusinessLogic.Queries.Messages;


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
        }
    }
}
