using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// terméklista
    /// </summary>
    public class Products
    {
        /// <summary>
        /// terméklista
        /// </summary>
        public List<Product> Items { get; set; }

        /// <summary>
        /// lapozó
        /// </summary>
        public Pager Pager { get; set; }

        /// <summary>
        /// elemek száma
        /// </summary>
        public long ListCount { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }
    }
}
