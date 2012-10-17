using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// kosár műveletekhez tartozó kérés objektum
    /// </summary>
    public class AddLine
    {
        public AddLine() : this(String.Empty, String.Empty, String.Empty, String.Empty, 0, String.Empty) { }

        public AddLine(string cartId, string productId, string language, string dataAreaId, int quantity, string visitorId)
        { 
            CartId = cartId;
            ProductId = productId;
            Language = language;
            DataAreaId = dataAreaId;
            Quantity = quantity;
            VisitorId = visitorId;
        }

        /// <summary>
        /// kosár azonosító
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
        /// mennyiség
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
