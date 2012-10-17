using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public class Completion
    {
        public Completion(string productId, string productName, string dataAreaId, long recId)
        {
            this.ProductId = productId;
            this.ProductName = productName;
            this.DataAreaId = dataAreaId;
            this.RecId = recId;
        }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string DataAreaId { get; set; }

        public long RecId { get; set; }
    }

    public class CompletionList : List<Completion>
    { 
    
    }
}
