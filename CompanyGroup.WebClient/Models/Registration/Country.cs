using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// ország
    /// </summary>
    public class Country
    {
        /// <summary>
        /// ország
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
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

    /// <summary>
    /// országok lista 
    /// </summary>
    public class Countries : List<Country> 
    {
        public void AddCountry(Country country)
        {
            this.Add(country);
        }
    }
}