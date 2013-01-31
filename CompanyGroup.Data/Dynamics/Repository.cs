using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.Dynamics
{
    /// <summary>
    /// NHibernate abstract repository
    /// </summary>
    public abstract class Repository : RepositoryBase
    {
        private NHibernate.ISession session;

        /// <summary>
        /// Repository példányosítása session-el
        /// </summary>
        /// <param name="session"></param>
        public Repository(NHibernate.ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            this.session = session;
        }

        /// <summary>
        /// NHibernate session kiolvasása
        /// </summary>
        public NHibernate.ISession Session
        {
            get { return session; }
        }

    }
}
