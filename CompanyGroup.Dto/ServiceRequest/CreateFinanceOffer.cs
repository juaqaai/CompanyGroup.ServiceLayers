using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// finanszírozási ajánlat létrehozás
    /// </summary>
    public class CreateFinanceOffer
    {
        public string VisitorId { get; set; }

        public string PersonName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string StatNumber { get; set; }

        public int NumOfMonth { get; set; }

        public int OfferId { get; set; }
    }
}
