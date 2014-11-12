namespace Racemate.Web.Areas.User.ViewModels.Home
{
    using System;
    using AutoMapper;
    using Racemate.Web.Infrastructure.Mapping;
    using Racemate.Data.Models;

    public class RaceThumbViewModel : IMapFrom<Race>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public DateTime DateTimeOfRace { get; set; }

        public string Type { get; set; }

        public int AvailableRacePositions { get; set; }

        public bool IsCanceled { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Race, RaceThumbViewModel>()
                .ForMember(dest => dest.Type,
                           opts => opts.MapFrom(src => src.Type.Name));
        }
    }
}