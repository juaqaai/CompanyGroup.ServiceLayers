using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "GetNewsletterCollection", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetNewsletterCollection
    {
        public GetNewsletterCollection() : this("", "", "") { }

        public GetNewsletterCollection(string language, string visitorId, string manufacturerId)
        { 
            Language = language;

            VisitorId = visitorId;

            ManufacturerId = manufacturerId;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Language", Order = 1)]
        public string Language { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 2)]
        public string VisitorId { get; set; }

        /// <summary>
        /// gyártó
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "ManufacturerId", Order = 3)]
        public string ManufacturerId { get; set; }
    }
}
