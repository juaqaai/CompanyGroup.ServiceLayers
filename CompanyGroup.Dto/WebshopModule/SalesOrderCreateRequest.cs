﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{

    /// <summary>
    /// látogatóhoz tartozó kosár feladása 
    /// </summary>
    public class SalesOrderCreateRequest
    {
        public SalesOrderCreateRequest() : this(0, "", "", "", 0, "", false, 0, 0, "", "") { }

        public SalesOrderCreateRequest(int cartId, string currency, string language, string visitorId, 
                                       long deliveryAddressRecId, string deliveryDate, bool deliveryRequest,
                                       int paymentTerm, int deliveryTerm, string customerOrderId, string customerOrderNote)
        {
            this.CartId = cartId;

            this.Currency = currency;

            this.Language = language;

            this.VisitorId = visitorId;

            this.DeliveryAddressRecId = deliveryAddressRecId;

            this.DeliveryDate = deliveryDate;

            this.DeliveryRequest = deliveryRequest;

            this.PaymentTerm = paymentTerm;

            this.DeliveryTerm = deliveryTerm;

            this.CustomerOrderId = customerOrderId;

            this.CustomerOrderNote = customerOrderNote;
        }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        public string Currency { get; set; }

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
        public string DeliveryDate { get; set; }

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
