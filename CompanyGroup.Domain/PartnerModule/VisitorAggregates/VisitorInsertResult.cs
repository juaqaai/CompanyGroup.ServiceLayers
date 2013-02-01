using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public class VisitorInsertResult
    {

        public VisitorInsertResult(int id, string visitorId)
        {
            this.Id = id;

            this.VisitorId = visitorId;
        }

        public VisitorInsertResult() : this(0, "") { }

        public int Id { get; set; }

        public string VisitorId { get; set; }
    }

}
