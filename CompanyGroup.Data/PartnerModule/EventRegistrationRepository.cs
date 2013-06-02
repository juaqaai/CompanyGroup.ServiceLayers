using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.PartnerModule
{
    /// <summary>
    /// eseményregisztráció repository
    /// </summary>
    public class EventRegistrationRepository : RepositoryBase, CompanyGroup.Domain.PartnerModule.IEventRegistrationRepository
    {
        public EventRegistrationRepository() { }

        /// <summary>
        /// web adatbázis kapcsolat
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession(); }
        }

        /// <summary>
        /// új regisztráció hozzáadás
        /// [InternetUser].[EventRegistrationAddNew]( @EventId VARCHAR (20), @EventName VARCHAR (80), @Xml NVARCHAR (MAX))
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventName"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public bool AddNew(string eventId, string eventName, string xml)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrEmpty(eventId), "EventId can not be null or empty");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.EventRegistrationAddNew").SetString("EventId", eventId)
                                                                                                       .SetString("EventName", eventName)
                                                                                                       .SetString("Xml", xml);

                long i = query.UniqueResult<long>();

                return (i > 0);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
        
}
