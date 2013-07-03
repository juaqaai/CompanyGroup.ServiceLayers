using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// vevő - árcsoport besorolás
    /// </summary>
    public class CustomerSpecialPrice
    {
        /// <summary>
        /// kulcs
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// látogató azonosító
        /// </summary>
        public int VisitorKey { get; set; }

        /// <summary>
        /// termék azonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// ár beállítás
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// vállalatkód
        /// </summary>
        public string DataAreaId { get; set; }

        /// <summary>
        /// vevőhöz tartozó speciális ár
        /// </summary>
        /// <param name="id"></param>
        /// <param name="visitorKey"></param>
        /// <param name="productId"></param>
        /// <param name="price"></param>
        /// <param name="dataAreaId"></param>
        public CustomerSpecialPrice(int id, int visitorKey, string productId, int price, string dataAreaId)
        {
            this.Id = id;

            this.VisitorKey = visitorKey;

            this.ProductId = productId;

            this.Price = price;

            this.DataAreaId = dataAreaId;
        }

        /// <summary>
        /// vevőhöz tartozó speciális ár
        /// </summary>
        public CustomerSpecialPrice() : this(0, 0, String.Empty, 0, String.Empty) { }

    }
}
