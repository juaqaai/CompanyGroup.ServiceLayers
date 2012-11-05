using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ShoppingCartOpenStatus
    {
        public ShoppingCartOpenStatus() 
        {
            this.IsOpen = false;
        }

        public bool IsOpen { get; set; }
    }
}
