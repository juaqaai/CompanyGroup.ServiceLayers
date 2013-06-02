using System;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// rendelés kérés adatokat összefogó POCO
    /// </summary>
    public class CreateFinanceOfferRequest
    {
        /// <summary>
        /// kosár azonosítója
        /// </summary>
        public int NumOfMonth { get; set; }
    }
}
