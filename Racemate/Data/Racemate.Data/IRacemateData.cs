namespace Racemate.Data
{
    using System.Data.Entity;
    using Racemate.Data.Models;
    using Racemate.Data.Common.Repository;

    public interface IRacemateData
    {
        // TODO: Create an abstraction
        DbContext Context { get; }

        IRepository<User> Users { get; }

        IRepository<InvitationCode> InvitationCodes { get; }

        IRepository<CarMake> CarMakes { get; }

        IRepository<CarModel> CarModels { get; }

        IRepository<Car> Cars { get; }

        IRepository<Race> Races { get; }

        IRepository<RaceType> RaceTypes { get; }

        IRepository<RaceRoutePoint> RaceRoutePoints { get; }

        IRepository<RaceParticipant> RaceParticipants { get; }

        IRepository<RaceSpectator> RaceSpectators { get; }

        IRepository<RaceChatMessage> RaceChatMessages { get; }

        IRepository<Report> Reports { get; }

        IRepository<Notification> Notifications { get; }

        int SaveChanges();
    }
}
