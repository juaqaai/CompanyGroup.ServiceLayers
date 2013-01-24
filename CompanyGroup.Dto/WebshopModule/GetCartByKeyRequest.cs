using System;

namespace CompanyGroup.Dto.WebshopModule
{
    public class GetCartByKeyRequest
    {
        public GetCartByKeyRequest() : this("", 0, "") { }

        public GetCartByKeyRequest(string language, int cartId, string visitorId)
        { 
            Language = language;
            CartId = cartId;
            VisitorId = visitorId;
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
    }
}
