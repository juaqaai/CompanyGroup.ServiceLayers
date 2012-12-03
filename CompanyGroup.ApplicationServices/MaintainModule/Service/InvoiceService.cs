using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.MaintainModule
{
    /// <summary>
    /// számlák szerviz karbantaró modul   
    /// </summary>
    public class InvoiceService : CompanyGroup.ApplicationServices.MaintainModule.IInvoiceService
    {
        private CompanyGroup.Domain.MaintainModule.IInvoiceRepository invoiceMaintainRepository;

        private CompanyGroup.Domain.PartnerModule.IInvoiceRepository invoicePartnerRepository;

        /// <summary>
        /// számlák szerviz (karbantartó szerviz, cache szervizműveletek)
        /// </summary>
        /// <param name="invoiceMaintainRepository"></param>
        public InvoiceService(CompanyGroup.Domain.MaintainModule.IInvoiceRepository invoiceMaintainRepository, CompanyGroup.Domain.PartnerModule.IInvoiceRepository invoicePartnerRepository)
        {
            if (invoiceMaintainRepository == null)
            {
                throw new ArgumentNullException("InvoiceRepository");
            }

            this.invoiceMaintainRepository = invoiceMaintainRepository;

            if (invoicePartnerRepository == null)
            {
                throw new ArgumentNullException("InvoicePartnerRepository");
            }

            this.invoicePartnerRepository = invoicePartnerRepository;
        }

        /// <summary>
        /// cache újratöltése (törlés, feltöltés)
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public bool ReFillCache(string dataAreaId)
        {
            return InsertList(dataAreaId, true);
        }

        /// <summary>
        /// cache feltöltése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public bool FillCache(string dataAreaId)
        { 
            return InsertList(dataAreaId, false);
        }

        /// <summary>
        /// lista beszúrása cache-be
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="clearCache"></param>
        /// <returns></returns>
        private bool InsertList(string dataAreaId, bool clearCache)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(dataAreaId), "The dataareaId cannot be null!");

            CompanyGroup.Helpers.DesignByContract.Require(dataAreaId.Equals(Domain.Core.Constants.DataAreaIdHrp) || dataAreaId.Equals(Domain.Core.Constants.DataAreaIdBsc), "The value of dataareaId can be hrp / bsc !");

            try
            {
                List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo> invoiceInfos = invoiceMaintainRepository.GetInvoiceDetailedLineInfo(dataAreaId);//CompanyGroup.Domain.Core.Constants.DataAreaIdHrp

                //számla info aggregátum elkészítése
                IEnumerable<IGrouping<string, CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo>> groupedLineInfos = invoiceInfos.GroupBy(x => x.InvoiceId).OrderBy(x => x.Key);   //IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>>

                List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = new List<CompanyGroup.Domain.PartnerModule.InvoiceInfo>();

                foreach (var lineInfo in groupedLineInfos)
                {
                    CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo = CompanyGroup.Domain.PartnerModule.InvoiceInfo.Create(lineInfo.ToList());

                    invoiceInfoList.Add(invoiceInfo);
                }

                if (clearCache)
                {
                    invoicePartnerRepository.RemoveItemsFromCollection(dataAreaId);
                }

                invoicePartnerRepository.InsertList(invoiceInfoList);

                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// vevőhöz tartozó számlák listája
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetInvoiceInfo(CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo request)
        //{
        //    Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

        //    try
        //    {
        //        List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> result = new List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>();

        //        //látogató azonosító alapján olvasása a cahce-ből, vagy a visitor repository-ból 
        //        CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

        //        //                if (visitor.IsValidLogin)
        //        //                {

        //        //visitor.

        //        List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo> invoiceInfos = invoiceRepository.GetInvoiceDetailedLineInfo(visitor.CompanyId, CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

        //        List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo> filteredInvoiceInfo;

        //        //lejárt, kifizetetlen
        //        if (CompanyGroup.Domain.PartnerModule.InvoicePaymentType.OverDue.Equals((CompanyGroup.Domain.PartnerModule.InvoicePaymentType)request.PaymentType))
        //        {
        //            //if (DateTime.Compare(t1, t2) >  0) Console.WriteLine("t1 > t2"); 
        //            //if (DateTime.Compare(t1, t2) == 0) Console.WriteLine("t1 == t2");
        //            //if (DateTime.Compare(t1, t2) < 0) Console.WriteLine("t1 < t2");

        //            filteredInvoiceInfo = invoiceInfos.Where(x => (DateTime.Compare(x.DueDate, DateTime.Today) < 0) && (x.InvoiceCredit > 0)).ToList();
        //        }
        //        //kifizetetlen
        //        else if (CompanyGroup.Domain.PartnerModule.InvoicePaymentType.Unpaid.Equals((CompanyGroup.Domain.PartnerModule.InvoicePaymentType)request.PaymentType))
        //        {
        //            filteredInvoiceInfo = invoiceInfos.Where(x => (x.InvoiceCredit > 0)).ToList();
        //        }
        //        //összes
        //        else
        //        {
        //            filteredInvoiceInfo = invoiceInfos;
        //        }

        //        //számla info aggregátum elkészítése
        //        IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo>> groupedLineInfos = filteredInvoiceInfo.GroupBy(x => x.InvoiceId).OrderBy(x => x.Key);   //IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>>

        //        List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = new List<CompanyGroup.Domain.PartnerModule.InvoiceInfo>();

        //        foreach (var lineInfo in groupedLineInfos)
        //        {
        //            CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo = CompanyGroup.Domain.PartnerModule.InvoiceInfo.Create(lineInfo.ToList());

        //            invoiceInfoList.Add(invoiceInfo);
        //        }

        //        result.AddRange(invoiceInfoList.ConvertAll(x => new InvoiceInfoToInvoiceInfo().Map(x)));
        //        //                }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
