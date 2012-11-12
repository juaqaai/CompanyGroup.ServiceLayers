using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// számla info lista
    /// </summary>
    public class InvoiceInfoList
    {
        public InvoiceInfoList(List<CompanyGroup.WebClient.Models.InvoiceInfo> items, Visitor visitor)
        {
            this.Items = items;

            this.ItemCount = items.Count;

            this.NettoSumCredit = String.Format("{0}", 0);

            this.Visitor = visitor;
        }

        public List<CompanyGroup.WebClient.Models.InvoiceInfo> Items { get; set; }

        public int ItemCount { get; set; }

        public string NettoSumCredit { get; set; }

        public bool HasItems { get { return Items.Count > 0; } }

        public Visitor Visitor { get; set; }
    }

}
