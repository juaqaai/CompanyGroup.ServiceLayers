using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// lízing opció
    /// </summary>
    public class LeasingOption
    {
        /// <summary>
        /// lízing opció
        /// </summary>
        public LeasingOption() : this(0, 0, String.Empty)
        { 
        }

        /// <summary>
        /// lízing opció
        /// </summary>
        /// <param name="financeparameterid"></param>
        /// <param name="numofmonth"></param>
        /// <param name="calculatedvalue"></param>
        public LeasingOption(int financeparameterid, int numofmonth, string calculatedvalue)
        {
            this.FinanceParameterId = financeparameterid;

            this.NumOfMonth = numofmonth;

            this.CalculatedValue = calculatedvalue;
        }

        /// <summary>
        /// választott futamidő azonosító
        /// </summary>
        public int FinanceParameterId { get; set; }

        /// <summary>
        /// választott futamidő hónapokban
        /// </summary>
        public int NumOfMonth { get; set; }

        /// <summary>
        /// kalkulált finanszírozandó összed
        /// </summary>
        public string CalculatedValue { get; set; }
    }

    /// <summary>
    /// lízing opciók
    /// </summary>
    public class LeasingOptions
    {
        /// <summary>
        /// lízing opciók
        /// </summary>
        public LeasingOptions() : this(new List<LeasingOption>(), String.Empty, String.Empty) { }

        /// <summary>
        /// lízing opciók
        /// </summary>
        /// <param name="items"></param>
        /// <param name="message"></param>
        /// <param name="sumTotal"></param>
        public LeasingOptions(List<LeasingOption> items, string message, string sumTotal)
        {
            this.Items = items;

            this.Message = message;

            this.SumTotal = sumTotal;
        }

        /// <summary>
        /// lízing opció elem lista 
        /// </summary>
        public List<LeasingOption> Items { get; set; }

        /// <summary>
        /// üzenet
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// finanszírozandó netto összeg 
        /// </summary>
        public string SumTotal { get; set; }
    }


}
