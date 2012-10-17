using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class LeasingOptionsToLeasingOptions
    {
        /// <summary>
        /// domain LeasingOptions -> DTO LeasingOptions  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.LeasingOptions Map(CompanyGroup.Domain.WebshopModule.LeasingOptions from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.LeasingOptions()
                {
                    Items = from.ToList().ConvertAll( x=> LeasingOptionToLeasingOption(x, from.Amount)),
                    Message = from.Message
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.LeasingOptions(); }
        }

        private static CompanyGroup.Dto.WebshopModule.LeasingOption LeasingOptionToLeasingOption(CompanyGroup.Domain.WebshopModule.LeasingOption from, double amount)
        {
            return new CompanyGroup.Dto.WebshopModule.LeasingOption() 
                       { 
                           CalculatedValue = CompanyGroup.Domain.WebshopModule.LeasingOptions.CalculateValue(from.PercentValue, amount), 
                           FinanceParameterId = from.Id, 
                           NumOfMonth = from.NumOfMonth 
                       };
        }


    }
}
