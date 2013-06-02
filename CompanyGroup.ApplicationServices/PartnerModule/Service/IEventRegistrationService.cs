using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public interface IEventRegistrationService
    {
        /// <summary>
        /// eseményregisztráció hozzáadás
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool AddNew(CompanyGroup.Dto.PartnerModule.EventRegistration request);
            
    }
}
