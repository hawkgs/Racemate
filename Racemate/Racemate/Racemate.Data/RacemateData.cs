namespace Racemate.Data
{
    using System;
    using System.Data.Entity;
    using System.Collections.Generic;
    using Racemate.Data.Repositories;
    using Racemate.Models;

    public class RacemateData : IRacemateData
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

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
                var newRepo = Activator.CreateInstance(typeof(EFRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepo);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
