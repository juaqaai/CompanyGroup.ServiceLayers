using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.WebClient.Models
{
    public class SaveCart
    {
        public SaveCart() : this(0, "") { }

        public SaveCart(int cartId, string name)
        {
            this.CartId = cartId;

            this.Name = name;
        }

        public int CartId { get; set; }

        public string Name { get; set; }
    }
}
