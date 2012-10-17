using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// előző böngésző betöltés 
    /// </summary>
    public class History : CompanyGroup.Domain.Core.ValueObject<History>
    {
        private HashSet<RequestParameter> requestParameters;

        /// <summary>
        /// history-ban található url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// naplózott kérés paraméterek (név - érték párok)
        /// </summary>
        public HashSet<RequestParameter> RequestParameters
        {
            get 
            {
                if (requestParameters == null)
                {
                    requestParameters = new HashSet<RequestParameter>();
                }
                return requestParameters;                
            }
            set
            {
                requestParameters = new HashSet<RequestParameter>(value);
            }
        }

        /// <summary>
        /// előfordulás ideje 
        /// </summary>
        public DateTime Date { get; set; }
    }
}
