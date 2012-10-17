using System;
using System.Collections.Generic;

namespace Newsletter.Repository
{
    /// <summary>
    /// NHibernate abstract repository
    /// </summary>
    public abstract class RepositoryBase
    {
        private NHibernate.ISession session;

        /// <summary>
        /// Repository példányosítása
        /// </summary>
        /// <param name="session">NHibernate session</param>
        public RepositoryBase(NHibernate.ISession session)
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
