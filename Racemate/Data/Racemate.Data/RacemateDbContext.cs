namespace Racemate.Data
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Racemate.Data.Models;
    using Racemate.Data.Migrations;
    using Racemate.Data.Common.Models;

    public class RacemateDbContext : IdentityDbContext<User>
    {
        public RacemateDbContext()
            : base("RacemateDbConnection", throwIfV1Schema: false)
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

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            this.ApplyDeletableEntityRules();

            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in this.ChangeTracker.Entries()
                .Where(
                e =>
                e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }

        private void ApplyDeletableEntityRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in this.ChangeTracker.Entries()
                .Where(e => e.Entity is IDeletableEntity && (e.State == EntityState.Deleted)))
            {
                var entity = (IDeletableEntity)entry.Entity;

                entity.DeletedOn = DateTime.Now;
                entity.IsDeleted = true;
                entry.State = EntityState.Modified;
            }
        }
    }
}
