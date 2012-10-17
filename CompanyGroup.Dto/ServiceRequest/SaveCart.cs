using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class SaveCart
    {
        public SaveCart() : this( "", "", "", "") { }

        public SaveCart(string language, string visitorId, string cartId, string name)
        {
            this.Language = language;

            this.VisitorId = visitorId;

            this.CartId = cartId;

            this.Name = name;
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
        public string CartId { get; set; }

        /// <summary>
        /// kosár neve
        /// </summary>
        public string Name { get; set; }
    }
}
