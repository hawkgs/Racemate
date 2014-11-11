namespace Racemate.Web.Areas.User.ViewModels.Invitations
{
    using System;
    using Racemate.Web.Infrastructure.Mapping;
    using Racemate.Data.Models;
    using AutoMapper;

    public class InvitationCodeViewModel : IMapFrom<InvitationCode>, IHaveCustomMappings
    {
        public string Code { get; set; }

        public string User { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<InvitationCode, InvitationCodeViewModel>()
                .ForMember(dest => dest.User,
                           opts => opts.MapFrom(src => src.User.UserName));
        }
    }
}