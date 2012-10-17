using System.Collections.Generic;
using CompanyGroup.Core.Utils;
using NHibernate;

namespace CompanyGroup.Data
{
    public class CustomerDao : CompanyGroup.Core.DataInterfaces.ICustomerDao
    {
        public List<CompanyGroup.Core.Domain.CustomerLetter> GetCustomerLetter(string customerId, string dataAreaId)
        {
            Check.Require(! string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

            Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            IQuery query = NHibernateSession.GetNamedQuery("InternetUser.CustomerLetterData")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Core.Domain.CustomerLetter).GetConstructors()[0]));

            return query.List<CompanyGroup.Core.Domain.CustomerLetter>() as List<CompanyGroup.Core.Domain.CustomerLetter>;
        }


        public List<CompanyGroup.Core.Domain.AddressZipCode> GetAddressZipCode(string prefix, string dataAreaId)
        {
            Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            IQuery query = NHibernateSession.GetNamedQuery("InternetUser.AddressZipCode")
                                            .SetString("Prefix", prefix)
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Core.Domain.AddressZipCode).GetConstructors()[0]));

            return query.List<CompanyGroup.Core.Domain.AddressZipCode>() as List<CompanyGroup.Core.Domain.AddressZipCode>;
        }

        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        private ISession NHibernateSession 
        {
            get { return NHibernateSessionManager.Instance.GetSession(); }
        }
    }
}
