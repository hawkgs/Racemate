namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class RaceRouteDataModel : IMapFrom<Race>, IHaveCustomMappings
    {
        [JsonProperty]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime StartDate { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime EndDate { get; set; }

        public bool IsFinished { get; set; }

        public bool IsCanceled { get; set; }

        public ICollection<RaceRoutePointDataModel> Routepoints { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Race, RaceRouteDataModel>()
                .ForMember(dest => dest.StartDate,
                            opts => opts.MapFrom(src => src.DateTimeOfRace))
                .ForMember(dest => dest.EndDate,
                            opts => opts.MapFrom(src => src.DateTimeOfRace.AddHours(src.Duration)))
                .ForMember(dest => dest.Routepoints,
                            opts => opts.MapFrom(src => src.Routepoints.AsQueryable()
                                                            .Project()
                                                            .To<RaceRoutePointDataModel>()));
        }
    }
}