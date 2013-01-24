using System;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// szállítási címek lekérdezés paramétereit összefogó típus
    /// </summary>
    public class GetDeliveryAddressesRequest
    {
        public GetDeliveryAddressesRequest() : this(String.Empty, String.Empty) { } 

        public GetDeliveryAddressesRequest(string dataAreaId, string visitorId)
        {
            this.DataAreaId = dataAreaId;

            this.VisitorId = visitorId;
        }

        public string DataAreaId { get; set; }

        public string VisitorId { get; set; }
    }
}
