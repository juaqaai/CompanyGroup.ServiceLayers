using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// számla info lista
    /// </summary>
    public class InvoiceInfo
    {
        public InvoiceInfo(List<CompanyGroup.Dto.PartnerModule.Invoice> items, 
                           CompanyGroup.Dto.PartnerModule.Pager pager, 
                           long listCount, 
                           string totalNettoCredit, 
                           string allOverdueDebts, 
                           Visitor visitor)
        {
            this.Items = items;

            this.Pager = pager;

            this.ListCount = listCount;

            this.Visitor = visitor;

            this.TotalNettoCredit = totalNettoCredit;

            this.AllOverdueDebts = allOverdueDebts;
        }

        /// <summary>
        /// számla fejléc lista
        /// </summary>
        public List<CompanyGroup.Dto.PartnerModule.Invoice> Items { get; set; }

        /// <summary>
        /// lapozó
        /// </summary>
        public CompanyGroup.Dto.PartnerModule.Pager Pager { get; set; }

        /// <summary>
        /// elemek száma
        /// </summary>
        public long ListCount { get; set; }

        /// <summary>
        /// összes tartozás
        /// </summary>
        public string TotalNettoCredit { get; set; }

        /// <summary>
        /// összes lejárt tartozás
        /// </summary>
        public string AllOverdueDebts { get; set; }

        /// <summary>
        /// látogató
        /// </summary>
        public Visitor Visitor { get; set; }
    }

}
