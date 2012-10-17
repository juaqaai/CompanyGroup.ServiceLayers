using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ShippingToShipping
    {
        /// <summary>
        /// domain kiszállítás cím -> DTO kiszállítási cím 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Shipping Map(CompanyGroup.Domain.WebshopModule.Shipping from)
        {
            try
            {
                return new Dto.WebshopModule.Shipping() 
                { 
                    AddrRecId = from.AddrRecId, 
                    City = from.City, 
                    Country = from.Country, 
                    DateRequested = from.DateRequested, 
                    InvoiceAttached = from.InvoiceAttached, 
                    Street = from.Street, 
                    ZipCode = from.ZipCode 
                };
            }
            catch
            {
                return new Dto.WebshopModule.Shipping();
            }
        }
    }
}
