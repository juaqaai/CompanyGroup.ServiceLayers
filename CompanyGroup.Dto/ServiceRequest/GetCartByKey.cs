using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "GetCartByKey", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetCartByKey
    {
        public GetCartByKey() : this("", "", "") { }

        public GetCartByKey(string language, string cartId, string visitorId)
        { 
            Language = language;
            CartId = cartId;
            VisitorId = visitorId;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string Language { get; set; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CartId", Order = 3)]
        public string CartId { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 1)]
        public string VisitorId { get; set; }
    }
}
