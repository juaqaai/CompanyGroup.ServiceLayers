using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// hívóparaméterek
    /// </summary>
    public class RequestParameter : CompanyGroup.Domain.Core.ValueObject<RequestParameter>
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
