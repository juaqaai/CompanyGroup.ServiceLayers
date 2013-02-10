﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class RemoveLine
    {
        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// kosár sor azonosító
        /// </summary>
        public int LineId { get; set; }

    }
}
