using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// finanszírozási ajánlat sor
    /// </summary>
    public class FinanceOfferLine
    {
        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// cikkszám
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// terméknév
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// mennyiség
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// egységár
        /// </summary>
        public double Price { get; set; }
    }
}
