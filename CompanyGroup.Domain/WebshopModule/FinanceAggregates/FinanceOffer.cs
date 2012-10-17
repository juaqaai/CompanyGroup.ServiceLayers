using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// finanszírozási ajánlat
    /// </summary>
    public class FinanceOffer
    {
        /// <summary>
        /// bérbevevő név
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// bérbevevő cím
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// bérbevevő telefon
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// bérbevevő cégjegyzékszám
        /// </summary>
        public string StatNumber { get; set; }

        /// <summary>
        /// futamidő
        /// </summary>
        public int NumOfMonth { get; set; }

        /// <summary>
        /// kosár elemei
        /// </summary>
        //public List<FinanceOfferLine> Lines { get; set; }

        /// <summary>
        /// finanszírozandó összeg
        /// </summary>
        //public double TotalAmount
        //{
        //    get 
        //    {
        //        double result = 0;

        //        Lines.ForEach(x => { result += x.Price; });

        //        return result;
        //    }
        //}
    }
}
