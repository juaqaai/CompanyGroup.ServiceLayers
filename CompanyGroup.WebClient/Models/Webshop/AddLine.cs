using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class AddLine
    {
        public AddLine()
        { 
            this.CartId = 0;

            this.ProductId = String.Empty;

            this.Quantity = 0;

            this.DataAreaId = String.Empty;
        }

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

        /// <summary>
        /// kosárhoz hozzáadni kívánt termék vállalata ('hrp', 'bsc')
        /// </summary>
        public string DataAreaId { get; set; }
    }
}
