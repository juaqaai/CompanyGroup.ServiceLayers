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
                           Visitor visitor)
        {
            this.Items = items;

            this.Pager = pager;

            this.ListCount = listCount;

            this.Visitor = visitor;
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
        /// látogató
        /// </summary>
        public Visitor Visitor { get; set; }
    }

}
