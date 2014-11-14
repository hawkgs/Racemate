namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using System;
    using AutoMapper;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class RaceDetailsViewModel : IMapFrom<Race>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string EncryptedId { get; set; }

        public DateTime DateTimeOfRace { get; set; }

        public int Duration { get; set; }

        public string OrganizerId { get; set; }

        public string Organizer { get; set; }

        public string Type { get; set; }

        public int AvailableRacePositions { get; set; }

        public string Address { get; set; }

        public float Distance { get; set; }

        public string Name { get; set; }

        public bool IsFinished { get; set; }

        public int? MoneyBet { get; set; }

        public string Password { get; set; }

        public bool IsCanceled { get; set; }

        public string Description { get; set; }

        public int SpectatorsCount { get; set; }

        // Post Properties >>>



        // <<< Post Properties

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Race, RaceDetailsViewModel>()
                .ForMember(dest => dest.Type,
                            opts => opts.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.Organizer,
                            opts => opts.MapFrom(src => src.Organizer.UserName))
                .ForMember(dest => dest.SpectatorsCount,
                            opts => opts.MapFrom(src => src.Spectators.Count));
        }
    }
}