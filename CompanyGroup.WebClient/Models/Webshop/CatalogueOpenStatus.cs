using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class CatalogueOpenStatus
    {
        public CatalogueOpenStatus()
        {
            this.IsOpen = false;
        }

        public bool IsOpen { get; set; }
    }
}
