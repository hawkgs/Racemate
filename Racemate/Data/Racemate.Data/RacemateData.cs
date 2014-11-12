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

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IRepository<InvitationCode> InvitationCodes
        {
            get { return this.GetRepository<InvitationCode>(); } 
        }

        public IRepository<CarMake> CarMakes
        {
            get { return this.GetRepository<CarMake>(); }
        }

        public IRepository<CarModel> CarModels
        {
            get { return this.GetRepository<CarModel>(); }
        }

        public IRepository<Car> Cars
        {
            get { return this.GetRepository<Car>(); }
        }

        public IRepository<Race> Races
        {
            get { return this.GetRepository<Race>(); }
        }

        public IRepository<RaceType> RaceTypes
        {
            get { return this.GetRepository<RaceType>(); }
        }

        public IRepository<RaceRoutePoint> RaceRoutePoints
        {
            get { return this.GetRepository<RaceRoutePoint>(); }
        }

        public IRepository<RaceParticipant> RaceParticipants
        {
            get { return this.GetRepository<RaceParticipant>(); }
        }

        public IRepository<RaceSpectator> RaceSpectators
        {
            get { return this.GetRepository<RaceSpectator>(); }
        }

        public IRepository<RaceChatMessage> RaceChatMessages
        {
            get { return this.GetRepository<RaceChatMessage>(); }
        }

        public IRepository<Report> Reports
        {
            get { return this.GetRepository<Report>(); }
        }

        public IRepository<Notification> Notifications
        {
            get { return this.GetRepository<Notification>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>()
            where T : class, IDeletableEntity
        {
            var typeOfRepository = typeof(T);

            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepo = Activator.CreateInstance(typeof(DeletableEntityRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepo);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
