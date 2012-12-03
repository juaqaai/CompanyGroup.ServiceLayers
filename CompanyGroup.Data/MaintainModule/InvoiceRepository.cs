﻿using System;
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
        /// vevőrendeléshez kapcsolódó műveletek 
        /// </summary>
        /// <param name="session"></param>
        public InvoiceRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// részletes számla sorok listája
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo> GetInvoiceDetailedLineInfo(string customerId, string dataAreaId)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(customerId), "customerId may not be null or empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_InvoiceList")
                                            .SetString("CustomerId", customerId)
                                            .SetString("DataAreaId", dataAreaId)
                                            .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo>() as List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo>;
        }
    }
}
