using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{

    public class SecondHandList
    {
        public string MinimumPrice { get; set; }

        public List<SecondHand> Items { get; set; }
    }

    public class SecondHand
    {

        public string ConfigId { get; set; }

        public string InventLocationId { get; set; }

        public int Quantity { get; set; }

        public string Price { get; set; }

        public string StatusDescription { get; set; }

        public string DataAreaId { get; set; }

        public int Id { get; set; }

        public string ProductId { get; set; }
    }
}
