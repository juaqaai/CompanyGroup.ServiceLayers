using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.WebClient.Models
{
    public class ManufacturerItem
    {
        public ManufacturerItem() : this(String.Empty, String.Empty) { }

        public ManufacturerItem(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
