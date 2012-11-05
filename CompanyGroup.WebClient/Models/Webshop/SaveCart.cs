using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.WebClient.Models
{
    public class SaveCart
    {
        public SaveCart() : this("") { }

        public SaveCart(string name)
        {
            //this.CartId = cartId;

            this.Name = name;
        }

        //public string CartId { get; set; }

        public string Name { get; set; }
    }
}
