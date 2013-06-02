using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// eseményregisztráció repository interfész
    /// </summary>
    public interface IEventRegistrationRepository
    {
        /// <summary>
        /// új regisztráció hozzáadás
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventName"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        bool AddNew(string eventId, string eventName, string xml);
    }
}
