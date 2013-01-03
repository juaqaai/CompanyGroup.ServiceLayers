using System;

namespace CompanyGroup.Domain.WebshopModule
{
    public class Stock
    {
        public Stock(int inner, int outer) 
        {
            this.Inner = inner;
            this.Outer = outer;
        }

        public Stock() : this(0, 0) { }

        public int Inner { get; set; }

        public int Outer { get; set; }
    }
}
