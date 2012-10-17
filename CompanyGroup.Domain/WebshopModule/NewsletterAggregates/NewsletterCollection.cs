using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// weben aktív hírlevél lista
    /// </summary>
    public class NewsletterCollection
    {
        /// <summary>
        /// konstruktor hírlevél listával
        /// </summary>
        /// <param name="newsletters"></param>
        public NewsletterCollection(List<Newsletter> newsletters)
        {
            this.Newsletters = newsletters;
        }

        /// <summary>
        /// hírlevelek lista
        /// </summary>
        public List<Newsletter> Newsletters { get; set; }

        /// <summary>
        /// listaelemek száma
        /// </summary>
        public int ItemCount { get { return this.Newsletters.Count; } }

        /// <summary>
        /// a cikk szerepel-e az aktív hírlevél listában?
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool IsInNewsletter(Product product)
        {
            return this.Newsletters.Exists(x => x.ProductId.Equals(product.ProductId));
        }
    }
}
