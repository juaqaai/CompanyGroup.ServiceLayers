using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    public class InvoiceInfo : List<Invoice>
    {
        /// <summary>
        /// listaelem számláló
        /// </summary>
        public long ListCount { get; set; }

        /// <summary>
        /// lista lapozó
        /// </summary>
        public Pager Pager { get; set; }

        /// <summary>
        /// aktuális sorrend 
        /// </summary>
        public int CurrentSequence { get; set; }

        /// <summary>
        /// lista nyitott állapotban van-e?
        /// </summary>
        public bool ListStatusOpen { get; set; }

        /// <summary>
        /// konstruktor lapozóval  
        /// </summary>
        /// <param name="listCount"></param>
        /// <param name="pager"></param>
        /// <param name="currentSequence"></param>
        /// <param name="listStatusOpen"></param>
        public InvoiceInfo(int listCount, Pager pager, int currentSequence, bool listStatusOpen)
        {
            ListCount = listCount;

            this.Pager = pager;

            this.CurrentSequence = currentSequence;

            this.ListStatusOpen = listStatusOpen;
        }

        /// <summary>
        /// összes tartozás
        /// </summary>
        public decimal TotalNettoCredit 
        {
            get { return this.Sum(x => x.Header.InvoiceCredit); } 
        }

        /// <summary>
        /// összes lejárt tartozás
        /// </summary>
        public decimal AllOverdueDebts
        {
            get { return this.Where(x => x.Header.OverDue).Sum(x => x.Header.InvoiceCredit); }
        }
    }
}
