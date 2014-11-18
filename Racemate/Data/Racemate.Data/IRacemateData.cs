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

        IDeletableEntityRepository<InvitationCode> InvitationCodes { get; }

        IDeletableEntityRepository<CarMake> CarMakes { get; }

        IDeletableEntityRepository<CarModel> CarModels { get; }

        IDeletableEntityRepository<Car> Cars { get; }

        IDeletableEntityRepository<Race> Races { get; }

        IDeletableEntityRepository<RaceType> RaceTypes { get; }

        IDeletableEntityRepository<RaceRoutePoint> RaceRoutePoints { get; }

        IDeletableEntityRepository<RaceParticipant> RaceParticipants { get; }

        IDeletableEntityRepository<RaceSpectator> RaceSpectators { get; }

        IDeletableEntityRepository<RaceChatMessage> RaceChatMessages { get; }

        IDeletableEntityRepository<Report> Reports { get; }

        IDeletableEntityRepository<Notification> Notifications { get; }

        int SaveChanges();
    }
}
