namespace Racemate.Data
{
    using Racemate.Models;
    using Racemate.Data.Repositories;

    public interface IRacemateData
    {
        IRepository<User> Users { get; }

        IRepository<InvitationCode> InvitationCodes { get; }

        IRepository<CarMake> CarMakes { get; }

        IRepository<CarModel> CarModels { get; }

        IRepository<Car> Cars { get; }

        IRepository<Race> Races { get; }

        IRepository<RaceRoutePoint> RaceRoutePoints { get; }

        IRepository<RaceParticipant> RaceParticipants { get; }

        IRepository<RaceSpectator> RaceSpectators { get; }

        IRepository<RaceChatMessage> RaceChatMessages { get; }

        IRepository<Report> Reports { get; }

        IRepository<Notification> Notifications { get; }

        int SaveChanges();
    }
}
