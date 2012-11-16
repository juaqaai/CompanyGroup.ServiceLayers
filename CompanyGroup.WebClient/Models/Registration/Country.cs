using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class Country
    {
        public Country(string id, string name)
        {
            this.Id = id;

            this.Name = name;

            this.Selected = false;
        }

        public string Id {get; set;}

        public string Name { get; set; }

        public bool Selected { get; set; }
    }

    public class Countries : List<Country> 
    {
        public void AddCountry(Country country)
        {
            this.Add(country);
        }
    }
}