using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.PartnerModule
{
    public class ContactPersonRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.PartnerModule.IContactPersonRepository
    {
        /// <summary>
        /// kapcsolattartóhoz kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public ContactPersonRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// jelszómódosítás ellenörzése
        /// </summary>
        /// <param name="contactPersonId"></param>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.ChangePasswordVerify VerifyChangePassword(string contactPersonId, string userName, string oldPassword, string newPassword, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(contactPersonId), "contactPersonId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_VerifyChangePassword")
                                            .SetString("ContactPersonId", contactPersonId)
                                            .SetString("UserName", userName)
                                            .SetString("OldPassword", oldPassword)
                                            .SetString("NewPassword", newPassword)
                                            .SetString("DataAreaId", dataAreaId).SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.ChangePasswordVerify).GetConstructors()[0]));

            CompanyGroup.Domain.PartnerModule.ChangePasswordVerify result = query.UniqueResult<CompanyGroup.Domain.PartnerModule.ChangePasswordVerify>();

            return result;
        }
    }
}
