namespace Racemate.Web.Areas.Administration.ViewModels.CarModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AutoMapper;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class CarModelViewModel : IMapFrom<CarModel>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CarMakeId { get; set; }

        public IEnumerable<SelectListItem> Makes { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<CarModel, CarModelViewModel>().ReverseMap();
        }
    }
}