using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.PartnerModule
{
    public class ContactPersonRepository : CompanyGroup.Domain.PartnerModule.IContactPersonRepository
    {
        /// <summary>
        /// kapcsolattartóhoz kapcsolódó műveletek konstruktor
        /// </summary>
        /// <param name="session"></param>
        public ContactPersonRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession(); }
        }

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

        /// <summary>
        /// elfelejtett jelszóhoz tartozó felhasználónév ellenörzése
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.ForgetPasswordVerify VerifyForgetPassword(string userName, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(userName), "UserName may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "DataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_VerifyForgetPassword")
                                            .SetString("UserName", userName)
                                            .SetString("DataAreaId", dataAreaId).SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.ForgetPasswordVerify).GetConstructors()[0]));

            CompanyGroup.Domain.PartnerModule.ForgetPasswordVerify result = query.UniqueResult<CompanyGroup.Domain.PartnerModule.ForgetPasswordVerify>();

            return result;
        }

        /// <summary>
        /// kapcsolattartó lekérdezés
        /// InternetUser.ContactPersonSelect( @CustomerId nvarchar(20), @ContactPersonId nvarchar(20), @DataAreaId NVARCHAR(3) = 'hrp')
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.PartnerModule.ContactPerson GetContactPerson(string contactPersonId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(contactPersonId), "ContactPersonId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "DataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ContactPersonSelect")
                                                            .SetString("CustomerId", String.Empty)
                                                            .SetString("ContactPersonId", contactPersonId)
                                                            .SetString("DataAreaId", dataAreaId)
                                                            .SetResultTransformer(
                                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.PartnerModule.ContactPerson).GetConstructors()[0]));

            CompanyGroup.Domain.PartnerModule.ContactPerson contactPerson = query.UniqueResult<CompanyGroup.Domain.PartnerModule.ContactPerson>();

            return contactPerson;
        }
    }
}
