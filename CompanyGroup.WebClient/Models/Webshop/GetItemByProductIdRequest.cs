using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// termékelem lekérdezése termékazonosító és vállalatkód alapján
    /// </summary>
    public class GetItemByProductIdRequest
    {
        public GetItemByProductIdRequest() : this("", "") { }

        public GetItemByProductIdRequest(string productId, string dataAreaId)
        {
            this.ProductId = productId;

            this.DataAreaId = DataAreaId;
        }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// vállalatkód
        /// </summary>
        public string DataAreaId { get; set; }
    }
}