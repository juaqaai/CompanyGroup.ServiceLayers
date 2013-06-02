using System;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// finanszírozási ajánlat létrehozás
    /// </summary>
    public class CreateFinanceOfferRequest
    {
        public CreateFinanceOfferRequest() : this(String.Empty, String.Empty, 0, 0) { }

        public CreateFinanceOfferRequest(string visitorId, string language, int numOfMonth, int offerId)
        {
            this.VisitorId = visitorId;

            this.Language = language;

            this.NumOfMonth = numOfMonth;

            this.OfferId = offerId;
        }   

        public string VisitorId { get; set; }

        public string Language { get; set; }

        public int NumOfMonth { get; set; }

        public int OfferId { get; set; }
    }
}
