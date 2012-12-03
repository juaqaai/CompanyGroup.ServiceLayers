using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.MaintainModule
{
    /// <summary>
    /// AX számlaműveleteket végző osztály
    /// </summary>
    public class InvoiceRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.MaintainModule.IInvoiceRepository
    {
        /// <summary>
        /// vevői számlákhoz kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public InvoiceRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// részletes számla sorok listája AX-ből
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo> GetInvoiceDetailedLineInfo(string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_InvoiceList")
                                                            .SetString("CustomerId", String.Empty)
                                                            .SetString("DataAreaId", dataAreaId)
                                                            .SetResultTransformer(
                                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo>() as List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo>;
        }
    }
}
