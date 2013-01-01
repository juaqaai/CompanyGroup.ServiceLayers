using System;

namespace CompanyGroup.Domain.WebshopModule
{
    public class Garanty
    {
        public Garanty(string time, string mode)
        {
            this.Time = time;
            this.Mode = mode;
        }

        public Garanty() : this("", "") { }

        public string Time { get; set; }

        public string Mode { get; set; }
    }
}
