using System;
using System.Collections.Generic;

namespace Newsletter.Repository
{
    /// <summary>
    /// címzett repository
    /// </summary>
    public class Recipient : Newsletter.Repository.RepositoryBase, Newsletter.Repository.IRecipient
    {
        /// <summary>
        /// címzettekhez kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public Recipient(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// kiküldendő lista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Newsletter.Dto.Recipient> GetRecipientList(int id)
        {
            CompanyGroup.Helpers.DesignByContract.Require(id > 0, "NewsletterId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetNewsletterRecipientList")
                                            .SetInt32("HeaderId", id)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(Newsletter.Dto.Recipient).GetConstructors()[0]));

            return query.List<Newsletter.Dto.Recipient>() as List<Newsletter.Dto.Recipient>;        
        }

        /// <summary>
        /// hozzáadott címlista lekérdezése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<Newsletter.Dto.Address> GetExtraAddressList(string dataAreaId)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetNewsletterExtraAddressList")
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(Newsletter.Dto.Address).GetConstructors()[0]));

            return query.List<Newsletter.Dto.Address>() as List<Newsletter.Dto.Address>;        
        }

    }
}
