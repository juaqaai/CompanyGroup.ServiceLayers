using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class FinanceOfferToFinanceOffer
    {
        /// <summary>
        /// domain finanszírozási ajánlat cím -> DTO finanszírozási ajánlat
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.FinanceOffer Map(CompanyGroup.Domain.WebshopModule.FinanceOffer from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.FinanceOffer() 
                { 
                    Address = from.Address, 
                    NumOfMonth = from.NumOfMonth, 
                    PersonName = from.PersonName, 
                    Phone = from.Phone, 
                    StatNumber = from.StatNumber
                };
            }
            catch
            {
                return new CompanyGroup.Dto.WebshopModule.FinanceOffer();
            }
        }
    }
}
