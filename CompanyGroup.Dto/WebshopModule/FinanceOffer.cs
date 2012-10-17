using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "FinanceOffer", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class FinanceOffer
    {
        public FinanceOffer()
        {
            this.PersonName = String.Empty;
            this.Address = String.Empty;
            this.Phone = String.Empty;
            this.StatNumber = String.Empty;
            this.NumOfMonth = 0;
        }

        /// <summary>
        /// bérbevevő név
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PersonName", Order = 1)]
        public string PersonName { get; set; }

        /// <summary>
        /// bérbevevő cím
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Address", Order = 2)]
        public string Address { get; set; }

        /// <summary>
        /// bérbevevő telefon
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Phone", Order = 3)]
        public string Phone { get; set; }

        /// <summary>
        /// bérbevevő cégjegyzékszám
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "StatNumber", Order = 4)]
        public string StatNumber { get; set; }

        /// <summary>
        /// futamidő
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "NumOfMonth", Order = 5)]
        public int NumOfMonth { get; set; }
    }
}
