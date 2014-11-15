namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using AutoMapper;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class SpectatorViewModel : IMapFrom<RaceSpectator>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<RaceSpectator, SpectatorViewModel>()
                .ForMember(dest => dest.Id,
                           opts => opts.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Name,
                           opts => opts.MapFrom(src => src.User.UserName));
        }
    }
}