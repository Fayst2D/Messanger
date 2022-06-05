


using AutoMapper;
using Messanger.BusinessLogic.Commands.Messages;
using Messanger.BusinessLogic.Commands.UserChats;


namespace Messanger.Presentation.PresentationProfile
{
    public class PresentationProfile : Profile
    {
        public PresentationProfile()
        {
            CreateMap<SendMessageRequest,SendMessageCommand>();
            CreateMap<CreateChannelRequest,CreateChannelCommand>();
            CreateMap<JoinChannelRequest,JoinChannelCommand>();
        }
    }
}
