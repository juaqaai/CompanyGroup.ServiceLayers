using System;

namespace CompanyGroup.Domain.WebshopModule
{

    /// <summary>
    /// domain product price value object
    /// </summary>
    public class Prices
    {
        public Prices(int price1, int price2, int price3, int price4, int price5, string currency)
        { 
            this.Price1 = price1;
            this.Price2 = price2;
            this.Price3 = price3;
            this.Price4 = price4;
            this.Price5 = price5;
            this.Currency = currency;
        }

        public Prices() : this(0, 0, 0, 0, 0, String.Empty) { }

        public int Price1 { get; set; }

        public int Price2 { get; set; }

        public int Price3 { get; set; }

        public int Price4 { get; set; }

        public int Price5 { get; set; }

        public string Currency { get; set; }
    }
}
