namespace Racemate.Web.Areas.Administration.ViewModels.CarMakes
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using AutoMapper;
    using Racemate.Data.Models;
    using Racemate.Web.Infrastructure.Mapping;

    public class CarMakeViewModel : IMapFrom<CarMake>, IHaveCustomMappings
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Make Name")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<CarMake, CarMakeViewModel>().ReverseMap();
        }
    }
}