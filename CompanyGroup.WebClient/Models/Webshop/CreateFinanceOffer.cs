﻿using System;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// rendelés kérés adatokat összefogó POCO
    /// </summary>
    public class CreateOrder
    {
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

        /// <summary>
        /// kosár azonosítója
        /// </summary>
        public int CartId { get; set; }
    }
}
