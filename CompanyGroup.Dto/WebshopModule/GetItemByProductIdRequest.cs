using System;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// termékelem lekérdezés kérés adatait összefogó DTO
    /// </summary>
    public class GetItemByProductIdRequest
    {
        public GetItemByProductIdRequest() : this(String.Empty, String.Empty, String.Empty, String.Empty) { }

        public GetItemByProductIdRequest(string productId, string dataAreaId, string visitorId, string currency)
        {
            this.ProductId = ProductId;

            this.DataAreaId = DataAreaId;

            this.VisitorId = VisitorId;

            this.Currency = Currency;
        }

        public string ProductId { get; set; }

        public string DataAreaId { get; set; }

        public string VisitorId { get; set; }

        public string Currency { get; set; }
    }
}
