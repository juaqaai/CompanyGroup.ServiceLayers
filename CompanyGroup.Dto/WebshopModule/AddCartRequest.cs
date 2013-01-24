using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class AddCartRequest
    {
        public AddCartRequest() : this( "", "", "") { }

        public AddCartRequest(string language, string visitorId, string cartName)
        {
            this.Language = language;

            this.VisitorId = visitorId;

            this.CartName = cartName;
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
        /// kosárnév
        /// </summary>
        public string CartName { get; set; }

    }
}
