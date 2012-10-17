using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// tartós bérlet számítás legkissebb és legnagyobb értékét tartalmazó adattípus
    /// </summary>
    public class MinMaxLeasingValue
    {
        public MinMaxLeasingValue(int minValue, int maxValue)
        { 
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }
    }
}
