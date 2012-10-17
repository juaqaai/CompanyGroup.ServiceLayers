using System;
using System.Collections.Generic;

namespace CompanyGroup.Infrastructure
{
    public interface IUnitOfWork    //NHibernateSessionManager public methods
    {
        /// <summary>
        /// get thread-safe, lazy singleton
        /// </summary>
        static NHibernateSessionManager Instance { get; }

        /// <summary>
        /// get nhibernate session
        /// </summary>
        /// <returns></returns>
        NHibernate.ISession GetSession();

        /// <summary>
        /// flushes anything left in the session and closes the connection
        /// </summary>
        void CloseSession();

        /// <summary>
        /// begin nhibernate transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// commit nhibernate transaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool HasOpenTransaction();

        /// <summary>
        /// rollback nhibernate transaction
        /// </summary>
        void RollbackTransaction();
    }
}
