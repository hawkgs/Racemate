namespace Racemate.Web.Areas.User.ViewModels.Home
{
    using System;

    public abstract class RaceAbstractViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateTimeOfRace { get; set; }

        public int Duration { get; set; }
    }
}