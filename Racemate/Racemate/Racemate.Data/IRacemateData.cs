namespace Racemate.Data
{
    using Racemate.Models;
    using Racemate.Data.Repositories;

    public interface IRacemateData
    {
        IRepository<User> Users { get; }

        IRepository<InvitationCode> InvitationCodes { get; }

        int SaveChanges();
    }
}
