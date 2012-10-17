using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "LeasingOption", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class LeasingOption
    {
        int financeparameterid = 0;
        int numofmonth = 0;
        double calculatedvalue = 0;

        [System.Runtime.Serialization.DataMember(Name = "FinanceParameterId", Order = 1)]
        public int FinanceParameterId
        {
            get { return financeparameterid; }
            set { financeparameterid = value; }
        }

        [System.Runtime.Serialization.DataMember(Name = "NumOfMonth", Order = 2)]
        public int NumOfMonth
        {
            get { return numofmonth; }
            set { numofmonth = value; }
        }

        [System.Runtime.Serialization.DataMember(Name = "CalculatedValue", Order = 3)]
        public double CalculatedValue
        {
            get { return calculatedvalue; }
            set { calculatedvalue = value; }
        }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "LeasingOptions", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class LeasingOptions
    {
        public LeasingOptions()
        {
            this.Items = new List<LeasingOption>();

            this.Message = "";
        }

        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<LeasingOption> Items { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Message", Order = 2)]
        public string Message { get; set; }
    }


}
