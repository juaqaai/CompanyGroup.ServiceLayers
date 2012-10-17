using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "OrderInfo", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class OrderInfo
    {
        /// <summary>
        /// VR, vagy BR azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "SalesId", Order = 1)]
        public string SalesId { set; get; }

        /// <summary>
        /// bizonylat elkészülte
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CreatedDate", Order = 2)]
        public DateTime CreatedDate { set; get; }

        /// <summary>
        /// sorok
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Lines", Order = 3)]
        public List<OrderLineInfo> Lines { get; set; }
    }
}
