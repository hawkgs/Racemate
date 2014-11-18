namespace Racemate.Data
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using Racemate.Data.Common.Repository;
    using Racemate.Data.Models;
    using Racemate.Data.Common.Models;

    public class RacemateData : IRacemateData
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;

        public RacemateData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public DbContext Context
        {
            get { return this.context; }
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IDeletableEntityRepository<InvitationCode> InvitationCodes
        {
            get { return this.GetDeleteableRepository<InvitationCode>(); } 
        }

        public IDeletableEntityRepository<CarMake> CarMakes
        {
            get { return this.GetDeleteableRepository<CarMake>(); }
        }

        public IDeletableEntityRepository<CarModel> CarModels
        {
            get { return this.GetDeleteableRepository<CarModel>(); }
        }

        public IDeletableEntityRepository<Car> Cars
        {
            get { return this.GetDeleteableRepository<Car>(); }
        }

        public IDeletableEntityRepository<Race> Races
        {
            get { return this.GetDeleteableRepository<Race>(); }
        }

        public IDeletableEntityRepository<RaceType> RaceTypes
        {
            get { return this.GetDeleteableRepository<RaceType>(); }
        }

        public IDeletableEntityRepository<RaceRoutePoint> RaceRoutePoints
        {
            get { return this.GetDeleteableRepository<RaceRoutePoint>(); }
        }

        public IDeletableEntityRepository<RaceParticipant> RaceParticipants
        {
            get { return this.GetDeleteableRepository<RaceParticipant>(); }
        }

        public IDeletableEntityRepository<RaceSpectator> RaceSpectators
        {
            get { return this.GetDeleteableRepository<RaceSpectator>(); }
        }

        public IDeletableEntityRepository<RaceChatMessage> RaceChatMessages
        {
            get { return this.GetDeleteableRepository<RaceChatMessage>(); }
        }

        public IDeletableEntityRepository<Report> Reports
        {
            get { return this.GetDeleteableRepository<Report>(); }
        }

        public IDeletableEntityRepository<Notification> Notifications
        {
            get { return this.GetDeleteableRepository<Notification>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>()
            where T : class
        {
            var typeOfRepository = typeof(T);

            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepo = Activator.CreateInstance(typeof(GenericRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepo);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }

        private IDeletableEntityRepository<T> GetDeleteableRepository<T>()
            where T : class, IDeletableEntity
        {
            var typeOfRepository = typeof(T);

            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepo = Activator.CreateInstance(typeof(DeletableEntityRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepo);
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
