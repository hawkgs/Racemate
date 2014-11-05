namespace Racemate.Data
{
    using System.Data.Entity;
    using Racemate.Models;
    using Racemate.Data.Migrations;
    using Microsoft.AspNet.Identity.EntityFramework;
    
    public class RacemateDbContext : IdentityDbContext<User>
    {
        public RacemateDbContext()
            : base("RacemateConnectionString", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RacemateDbContext, Configuration>());
        }

        public static RacemateDbContext Create()
        {
            return new RacemateDbContext();
        }

        public IDbSet<InvitationCode> InvitationCodes { get; set; }

        public IDbSet<CarMake> CarMakes { get; set; }

        public IDbSet<CarModel> CarModels { get; set; }

        public IDbSet<Car> Cars { get; set; }

        public IDbSet<Race> Races { get; set; }

        public IDbSet<RaceRoutePoint> RaceRoutePoints { get; set; }

        public IDbSet<RaceParticipant> RaceParticipants { get; set; }

        public IDbSet<RaceSpectator> RaceSpectators { get; set; }

        public IDbSet<RaceChatMessage> RaceChatMessages { get; set; }

        public IDbSet<Report> Reports { get; set; }

        public IDbSet<Notification> Notifications { get; set; }
    }
}
