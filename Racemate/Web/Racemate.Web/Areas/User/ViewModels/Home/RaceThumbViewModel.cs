namespace Racemate.Web.Areas.User.ViewModels.Home
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class RaceThumbViewModel : RaceAbstractViewModel, IMapFrom<Race>, IHaveCustomMappings
    {
        public string Address { get; set; }

        public bool IsFinished { get; set; }

        public bool IsCanceled { get; set; }

        public string Type { get; set; }

        public int FreeRacePositions { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Race, RaceThumbViewModel>()
                .ForMember(dest => dest.Type,
                           opts => opts.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.FreeRacePositions,
                           opts => opts.MapFrom(src =>
                                   src.AvailableRacePositions - src.Participants
                                   .Where(p => !p.IsKicked && !p.IsDeleted).Count()));
        }
    }
}