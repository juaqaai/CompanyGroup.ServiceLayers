using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// kosár törlés művelethez tartozó kérés objektum
    /// </summary>
    public class RemoveLine
    {
        public RemoveLine() : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty) { }

        public RemoveLine(string cartId, string productId, string language, string dataAreaId, string visitorId)
        {
            CartId = cartId;

            ProductId = productId;

            Language = language;

            DataAreaId = dataAreaId;

            VisitorId = visitorId;
        }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string CartId { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// beállított nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// vállalat
        /// </summary>
        public string DataAreaId { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
