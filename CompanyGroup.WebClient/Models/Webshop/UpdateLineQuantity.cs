﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class UpdateLineQuantity
    {
        /// <summary>
        /// kosár azonosító
        /// </summary>
        //public string CartId { get; set; }

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
