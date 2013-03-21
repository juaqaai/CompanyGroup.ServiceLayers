using System;

namespace CompanyGroup.Dto.WebshopModule
{
    public class GetCartByKeyRequest
    {
        public GetCartByKeyRequest() : this("", 0, "", "") { }

        public GetCartByKeyRequest(string language, int cartId, string visitorId, string currency)
        { 
            this.Language = language;
            
            this.CartId = cartId;
            
            this.VisitorId = visitorId;

            this.Currency = currency;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }
    }
}
