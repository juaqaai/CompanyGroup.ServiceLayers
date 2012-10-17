using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class AssociateCart
    {
        public AssociateCart() : this("", "") { }

        public AssociateCart(string visitorId, string permanentId)
        {
            this.VisitorId = visitorId;

            this.PermanentId = permanentId;

            this.Language = "";
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
        /// látogató permanens azonosítója
        /// </summary>
        public string PermanentId { get; set; }

    }
}
