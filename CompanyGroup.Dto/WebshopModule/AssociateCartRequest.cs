using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class AssociateCartRequest
    {
        public AssociateCartRequest() : this("", "") { }

        public AssociateCartRequest(string visitorId, string permanentId)
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
