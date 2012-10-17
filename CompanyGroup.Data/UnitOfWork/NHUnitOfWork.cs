using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Infrastructure
{
    public sealed class NHUnitOfWork : IDisposable
    {
        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        private NHibernate.ISession NHibernateSession
        {
            get { return NHibernateSessionManager.Instance.GetSession(); }
        }

        /// <summary>
        /// Returns a strong typed persistent instance of the given named entity with the given identifier, or null if there is no such persistent instance.
        /// </summary>
        /// <typeparam name="T">The type of the given persistant instance.</typeparam>
        /// <param name="id">An identifier.</param>
        public T Get<T>(object id)
        {
            T returnVal = NHibernateSession.Get<T>(id);
            return returnVal;
        }

        /// <summary>
        /// Returns a list of all instances of type T from the database.
        /// </summary>
        /// <typeparam name="T">The type of the given persistant instance.</typeparam>
        public IList<T> GetAll<T>() where T : class
        {
            IList<T> returnVal = NHibernateSession.CreateCriteria<T>().List<T>();
            return returnVal;
        }

        /// <summary>
        /// IDisposable member
        /// </summary>
        public void Dispose()
        {
            NHibernateSession.Close();
        }
    }

    //public sealed class UnitOfWork : IDisposable
    //{
    //    private readonly ISession session;
    //    private ITransaction transaction;

    //    public UnitOfWork()
    //    {
    //        session = SessionManager.Instance.Session; //this may be an already open session...
    //        session.FlushMode = FlushMode.Auto; //default
    //        transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
    //    }

    //    public ISession Current
    //    {
    //        get { return session; }
    //    }

    //    /// <summary>
    //    /// Commits this instance.
    //    /// </summary>
    //    public void Commit()
    //    {
    //        //becuase flushMode is auto, this will automatically commit when disposed
    //        if (!transaction.IsActive)
    //            throw new InvalidOperationException("No active transaction");
    //        transaction.Commit();
    //        //start a new transaction
    //        transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
    //    }

    //    /// <summary>
    //    /// Rolls back this instance. You should probably close session.
    //    /// </summary>
    //    public void Rollback()
    //    {
    //        if (transaction.IsActive) transaction.Rollback();
    //    }

    //    #region IDisposable Members

    //    public void Dispose()
    //    {
    //        if (session != null) session.Close();
    //    }

    //    #endregion
    //}
}
