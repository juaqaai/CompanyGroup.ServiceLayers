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
                List<CompanyGroup.Dto.WebshopModule.LeasingOption> items = from.ToList().ConvertAll( x => LeasingOptionToLeasingOption(x) ); 

                string sumTotal = String.Format("{0:0,0.00}", from.Amount);

                return new CompanyGroup.Dto.WebshopModule.LeasingOptions(items, from.Message, sumTotal);
            }
            catch { return new CompanyGroup.Dto.WebshopModule.LeasingOptions(); }
        }

        /// <summary>
        ///  domain LeasingOption -> DTO LeasingOption  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        private CompanyGroup.Dto.WebshopModule.LeasingOption LeasingOptionToLeasingOption(CompanyGroup.Domain.WebshopModule.LeasingOption from)
        {
            string calculatedValue = String.Format("{0:0,0.00}", from.CalculatedValue);

            return new CompanyGroup.Dto.WebshopModule.LeasingOption(from.Id, from.NumOfMonth, calculatedValue);
        }


    }
}
