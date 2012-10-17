using System.Runtime.Remoting.Messaging;
using System.Web;
using CompanyGroup.Domain.Utils;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;

namespace CompanyGroup.Data
{
    /// <summary>
    /// NHibernte singleton session. (http://www.typemock.com) for testing.
    /// </summary>
    public sealed class NHibernateSessionManager
    {

        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";

        private const string SESSION_KEY = "CONTEXT_SESSION";

        private ISessionFactory sessionFactory;

        /// <summary>
        /// get thread-safe, lazy singleton  http://www.yoda.arachsys.com/csharp/singleton.html
        /// </summary>
        public static NHibernateSessionManager Instance { get { return Nested.NHibernateSessionManager; } }

        /// <summary>
        /// Private contructor - Initializes the NHibernate session factory.
        /// </summary>
        private NHibernateSessionManager() { InitSessionFactory(); }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            /// <summary>
            /// 
            /// </summary>
            static Nested() { }

            internal static readonly NHibernateSessionManager NHibernateSessionManager = new NHibernateSessionManager();
        }

        /// <summary>
        /// session factory initialization
        /// </summary>
        private void InitSessionFactory() 
        {
            sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }

        /// <summary>
        /// get nhibernate session
        /// </summary>
        /// <returns></returns>
        public ISession GetSession() { return GetSession(null); }

        /// <summary>
        /// gets a session with or without an interceptor, this method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private NHibernate.ISession GetSession(IInterceptor interceptor) 
        {
            NHibernate.ISession session = ContextSession;

            if (session == null)
            {
                session = (interceptor != null) ? sessionFactory.OpenSession(interceptor) : sessionFactory.OpenSession();

                ContextSession = session;
            }

            Check.Ensure(session != null, "session was null");

            return session;
        }

        /// <summary>
        /// flushes anything left in the session and closes the connection
        /// </summary>
        public void CloseSession() 
        {
            ISession session = ContextSession;

            if (session != null && session.IsOpen) {
                session.Flush();
                session.Close();
            }

            ContextSession = null;
        }

        /// <summary>
        /// begin nhibernate transaction
        /// </summary>
        public void BeginTransaction() 
        {
            ITransaction transaction = ContextTransaction;

            if (transaction == null) 
            {
                transaction = GetSession().BeginTransaction();
                ContextTransaction = transaction;
            }
        }

        /// <summary>
        /// commit nhibernate transaction
        /// </summary>
        public void CommitTransaction() 
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction()) 
                {
                    transaction.Commit();
                    ContextTransaction = null;
                }
            }
            catch (HibernateException) 
            {
                RollbackTransaction();
                throw;
            }
        }

        public bool HasOpenTransaction() 
        {
            ITransaction transaction = ContextTransaction;

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        /// <summary>
        /// rollback nhibernate transaction
        /// </summary>
        public void RollbackTransaction() 
        {
            ITransaction transaction = ContextTransaction;

            try 
            {
                if (HasOpenTransaction())
                {
                    transaction.Rollback();
                }

                ContextTransaction = null;
            }
            finally 
            {
                CloseSession();
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
        /// specific <see cref="CallContext" />.
        /// </summary>
        private ITransaction ContextTransaction
        {
            get
            {
                return (IsInWebContext()) ? (ITransaction)HttpContext.Current.Items[TRANSACTION_KEY] : (ITransaction)CallContext.GetData(TRANSACTION_KEY);
            }
            set 
            {
                if (IsInWebContext()) 
                {
                    HttpContext.Current.Items[TRANSACTION_KEY] = value;
                }
                else
                {
                    CallContext.SetData(TRANSACTION_KEY, value);
                }
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
        /// specific <see cref="CallContext" />.
        /// </summary>
        private ISession ContextSession 
        {
            get 
            {
                return (IsInWebContext()) ? (ISession)HttpContext.Current.Items[SESSION_KEY] : (ISession)CallContext.GetData(SESSION_KEY);
            }
            set 
            {
                if (IsInWebContext())
                {
                    HttpContext.Current.Items[SESSION_KEY] = value;
                }
                else 
                {
                    CallContext.SetData(SESSION_KEY, value);
                }
            }
        }

        private bool IsInWebContext() 
        {
            return HttpContext.Current != null;
        }
    }
}
