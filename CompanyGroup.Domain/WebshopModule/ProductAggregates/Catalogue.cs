using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// összefogja a Products és a Structure információkat
    /// </summary>
    public class Catalogue
    {
        public Catalogue(Products products, Structures structures)
        {
            this.Products = products;

            this.Structures = structures;
        }

        public Products Products { get; set; }

        public Structures Structures { get; set; }
    }
}
