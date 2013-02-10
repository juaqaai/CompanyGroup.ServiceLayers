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
    public class NHibernateSessionManager
    {

        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";

        private const string SESSION_KEY = "CONTEXT_SESSION";

        private ISessionFactory sessionFactory;

        #region Thread-safe, lazy Singleton

        /// <summary>
        /// get thread-safe, lazy singleton  http://www.yoda.arachsys.com/csharp/singleton.html
        /// </summary>
        public static NHibernateSessionManager Instance { get { return Nested.NHibernateSessionManager; } }

        /// <summary>
        /// Private contructor - Initializes the NHibernate session factory.
        /// </summary>
        private NHibernateSessionManager() { }  //InitSessionFactory(); 

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

        #endregion

        /// <summary>
        /// This method attempts to find a session factory
        /// in the <see cref="HttpRuntime.Cache" /> 
        /// via its config file path; if it can't be
        /// found it creates a new session factory and adds
        /// it the cache. Note that even though this uses HttpRuntime.Cache,
        /// it should still work in Windows applications; see
        /// http://www.codeproject.com/csharp/cacheinwinformapps.asp
        /// for an examination of this.
        /// </summary>
        /// <param name="sessionFactoryConfigPath">Path location
        /// of the factory config</param>
        private ISessionFactory GetSessionFactoryFor(string sessionFactoryConfigPath)
        {
            if (string.IsNullOrEmpty(sessionFactoryConfigPath))
            {
                throw new System.ArgumentNullException("sessionFactoryConfigPath may not be null nor empty");
            }

            //Attempt to retrieve a cached SessionFactory from the HttpRuntime's cache.
            ISessionFactory sessionFactory = (ISessionFactory) HttpRuntime.Cache.Get(sessionFactoryConfigPath);

            //  Failed to find a cached SessionFactory so make a new one.
            if (sessionFactory == null)
            {
                if (!System.IO.File.Exists(sessionFactoryConfigPath))
                {
                    throw new System.ApplicationException("The config file at '" + sessionFactoryConfigPath + "' could not be found");
                }

                NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();

                cfg.Configure(sessionFactoryConfigPath);

                //  Now that we have our Configuration object, create a new SessionFactory
                sessionFactory = cfg.BuildSessionFactory();

                if (sessionFactory == null)
                {
                    throw new System.InvalidOperationException("cfg.BuildSessionFactory() returned null.");
                }

                HttpRuntime.Cache.Add(sessionFactoryConfigPath, sessionFactory, null, System.DateTime.Now.AddDays(7), System.TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
            }

            return sessionFactory;
        }

        public void RegisterInterceptorOn(string sessionFactoryConfigPath, IInterceptor interceptor)
        {
            ISession session = (ISession)contextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen)
            {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSessionFrom(sessionFactoryConfigPath, interceptor);
        }

        /// <summary>
        /// web interfész adatbázis session
        /// </summary>
        /// <returns></returns>
        public ISession GetWebInterfaceSession()
        {
            return GetSessionFrom(Helpers.ConfigSettingsParser.GetString("WebInterfaceSessionFactoryConfigPath"));
        }

        /// <summary>
        /// extract interface adatbázis session
        /// </summary>
        /// <returns></returns>
        public ISession GetExtractInterfaceSession()
        {
            return GetSessionFrom(Helpers.ConfigSettingsParser.GetString("ExtractInterfaceSessionFactoryConfigPath"));
        }

        private ISession GetSessionFrom(string sessionFactoryConfigPath)
        {
            return GetSessionFrom(sessionFactoryConfigPath, null);
        }

        private ISession GetSessionFrom(string sessionFactoryConfigPath, IInterceptor interceptor)
        {
            ISession session = (ISession)contextSessions[sessionFactoryConfigPath];

            if (session == null)
            {
                if (interceptor != null)
                {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession(interceptor);
                }
                else
                {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession();
                }

                session.CacheMode = CacheMode.Put;

                contextSessions[sessionFactoryConfigPath] = session;
            }

            if (session == null)
            {
                throw new System.ApplicationException("session was null");
            }

            return session;
        }

        public void CloseSessionOn(string sessionFactoryConfigPath)
        {
            ISession session = (ISession)contextSessions[sessionFactoryConfigPath];
            contextSessions.Remove(sessionFactoryConfigPath);

            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }

        public void BeginTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)contextTransactions[sessionFactoryConfigPath];

            if (transaction == null)
            {
                transaction = GetSessionFrom(sessionFactoryConfigPath).BeginTransaction();

                contextTransactions.Add(sessionFactoryConfigPath, transaction);
            }
        }

        public void CommitTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)contextTransactions[sessionFactoryConfigPath];

            try
            {
                if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                {
                    transaction.Commit();

                    contextTransactions.Remove(sessionFactoryConfigPath);
                }
            }
            catch (HibernateException)
            {
                RollbackTransactionOn(sessionFactoryConfigPath);

                throw;
            }
        }

        public void RollbackTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)contextTransactions[sessionFactoryConfigPath];

            try
            {
                contextTransactions.Remove(sessionFactoryConfigPath);

                if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                CloseSessionOn(sessionFactoryConfigPath);
            }
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one transaction per database 
        /// persisted at any one time. The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.
        /// </summary>
        private System.Collections.Hashtable contextTransactions
        {
            get
            {
                if (CallContext.GetData("CONTEXT_TRANSACTIONS") == null)
                {
                    CallContext.SetData("CONTEXT_TRANSACTIONS", new System.Collections.Hashtable());
                }

                return (System.Collections.Hashtable) CallContext.GetData("CONTEXT_TRANSACTIONS");
            }
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one session per database 
        /// persisted at any one time. The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.
        /// </summary>
        private System.Collections.Hashtable contextSessions
        {
            get
            {
                if (CallContext.GetData("CONTEXT_SESSIONS") == null)
                {
                    CallContext.SetData("CONTEXT_SESSIONS", new System.Collections.Hashtable());
                }

                return (System.Collections.Hashtable) CallContext.GetData("CONTEXT_SESSIONS");
            }
        }

        /***/

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
