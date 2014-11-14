namespace Racemate.Web.Areas.User.ViewModels.Race
{
    using System;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class UserCarsSelectViewModel : IMapFrom<Car>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Car { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Car, UserCarsSelectViewModel>()
                .ForMember(dest => dest.Car,
                           opts => opts.MapFrom(src => String.Format("{0} {1}", src.Model.CarMake.Name, src.Model.Name)));
        }
    }
}