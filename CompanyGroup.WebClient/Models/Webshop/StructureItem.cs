using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class StructureItem
    {
        public StructureItem() : this(String.Empty, String.Empty) { }

        public StructureItem(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
