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
        /// konstruktor lapozóval  
        /// </summary>
        /// <param name="listCount"></param>
        /// <param name="pager"></param>
        /// <param name="currentSequence"></param>
        public InvoiceInfo(int listCount, Pager pager, int currentSequence)
        {
            ListCount = listCount;

            this.Pager = pager;

            this.CurrentSequence = currentSequence;
        }

        /// <summary>
        /// összes tartozás
        /// </summary>
        //public decimal TotalNettoCredit { get; set; }

        /// <summary>
        /// összes lejárt tartozás
        /// </summary>
        //public decimal AllOverdueDebts { get; set; }
    }
}
