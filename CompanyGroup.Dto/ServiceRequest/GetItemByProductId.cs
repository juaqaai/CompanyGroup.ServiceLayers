using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// termékelem lekérdezés kérés objektum adatait összefogó DTO
    /// </summary>
    [System.Runtime.Serialization.DataContract(Name = "GetItemByProductId", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class GetItemByProductId
    {
        [System.Runtime.Serialization.DataMember(Name = "ProductId", Order = 4)]
        public string ProductId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "DataAreaId", Order = 3)]
        public string DataAreaId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 1)]
        public string VisitorId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Currency", Order = 2)]
        public string Currency { get; set; }
    }
}
