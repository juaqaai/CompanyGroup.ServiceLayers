﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// látogatóhoz tartozó kosár feladása 
    /// </summary>
    public class SalesOrderCreate
    {
        public SalesOrderCreate() : this(0, "", "") { }

        public SalesOrderCreate(int cartId, string language, string visitorId)
        {
            CartId = cartId;

            Language = language;

            VisitorId = visitorId;
        }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// látogató azonosító
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// szállítási cím kulcsa
        /// </summary>
        public long DeliveryAddressRecId { get; set; }

        /// <summary>
        /// szállítás időpontja
        /// </summary>
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// szállítást kért-e
        /// </summary>
        public bool DeliveryRequest { get; set; }

        /// <summary>
        /// 1: átut, 2: KP, 3: előreut, 4: utánvét
        /// </summary>
        public int PaymentTerm { get; set; }

        /// <summary>
        /// 1: raktár, 2: kiszállítás
        /// </summary>
        public int DeliveryTerm { get; set; }

        /// <summary>
        /// idegen rendelés azonosító 
        /// </summary>
        public string CustomerOrderId { get; set; }

        /// <summary>
        /// idegen rendeléshez kapcsolódó feljegyzés
        /// </summary>
        public string CustomerOrderNote { get; set; }
    }
}
