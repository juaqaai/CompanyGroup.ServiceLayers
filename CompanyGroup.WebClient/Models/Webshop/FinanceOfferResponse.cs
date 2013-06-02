using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// lízing opciók válaszüzenet
    /// </summary>
    public class FinanceOfferResponse 
    {
        public FinanceOfferResponse() : this(new List<CompanyGroup.Dto.WebshopModule.LeasingOption>(), String.Empty, String.Empty) { }

        public FinanceOfferResponse(List<CompanyGroup.Dto.WebshopModule.LeasingOption> items, string message, string sumTotal)
        {
            this.Items = items;

            this.Message = message;

            this.SumTotal = sumTotal;
        }

        public List<CompanyGroup.Dto.WebshopModule.LeasingOption> Items { get; set; }

        public string Message { get; set; }

        public string SumTotal { get; set; }

    }
}