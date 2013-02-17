using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// megrendelés információ  
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// VR, vagy BR azonosító
        /// </summary>
        public string SalesId { set; get; }

        /// <summary>
        /// bizonylat elkészülte
        /// </summary>
        public DateTime CreatedDate { set; get; }

        /// <summary>
        /// vállalatkód, ahonnan a rendelés származik
        /// </summary>
        public string DataAreaId { set; get; }

        /// <summary>
        /// fizetés módja (Átutalás 8 nap)
        /// </summary>
        public string Payment { set; get; }

        /// <summary>
        /// Standard / Csereigazolás
        /// </summary>
        public string SalesHeaderType { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int HeaderSalesStatus { set; get; }

        /// <summary>
        /// sorok
        /// </summary>
        public List<OrderLineInfo> Lines { get; set; }
    }
}
