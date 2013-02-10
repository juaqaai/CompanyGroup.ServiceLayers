using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class AddLine
    {
        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// mennyiség
        /// </summary>
        public int Quantity { get; set; }
    }
}
