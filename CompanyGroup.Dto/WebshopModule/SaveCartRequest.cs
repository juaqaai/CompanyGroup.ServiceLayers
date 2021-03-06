﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class SaveCartRequest
    {
        public SaveCartRequest() : this( "", "", 0, "", "") { }

        public SaveCartRequest(string language, string visitorId, int cartId, string name, string currency)
        {
            this.Language = language;

            this.VisitorId = visitorId;

            this.CartId = cartId;

            this.Name = name;

            this.Currency = currency;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// kosár neve
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency { get; set; }
    }
}
