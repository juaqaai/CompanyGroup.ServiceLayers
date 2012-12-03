using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.MaintainModule
{
    public class ProductDescription
    {
        public ProductDescription(string productId, string description, string langId)
        {
            this.ProductId = productId;

            this.Description = description;

            this.LangId = langId;
        }

        public string ProductId { get; set; }

        public string Description { get; set; }

        public string LangId { get; set; }
    }
}
