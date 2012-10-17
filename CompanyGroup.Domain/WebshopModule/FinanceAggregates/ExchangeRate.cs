using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// átváltási ráta
    /// </summary>
    public class ExchangeRate
    {
        public ExchangeRate(DateTime fromDate, string currencyCode, decimal rate, string dataAreaId)
        { 
            this.FromDate = fromDate;
            
            this.CurrencyCode = currencyCode;
            
            this.Rate = rate;
        
            this.DataAreaId = dataAreaId;
        }

        public DateTime FromDate { get; set; }	

        public string CurrencyCode { get; set; }	
        
        public decimal Rate { get; set; }

        public string DataAreaId { get; set; }	

    }
}
