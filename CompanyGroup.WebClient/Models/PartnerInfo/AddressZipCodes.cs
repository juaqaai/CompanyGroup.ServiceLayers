using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class AddressZipCodes : CompanyGroup.Dto.PartnerModule.AddressZipCodes
    {
        public AddressZipCodes(CompanyGroup.Dto.PartnerModule.AddressZipCodes addressZipCodes)
        {
            this.Items = addressZipCodes.Items;
        }
    }
}