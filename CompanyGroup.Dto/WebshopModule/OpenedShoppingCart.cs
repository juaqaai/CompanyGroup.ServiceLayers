using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    public class OpenedShoppingCart
    {
        public OpenedShoppingCart()
        {
            this.Id = 0;
            this.Name = String.Empty;
            this.Active = false;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
