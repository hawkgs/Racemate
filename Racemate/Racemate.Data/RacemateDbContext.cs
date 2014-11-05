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
    }
}
