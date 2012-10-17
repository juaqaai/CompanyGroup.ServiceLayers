using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    ///  domain CompanyGroup.Domain.WebshopModule.SecondHand list  -> CompanyGroup.Dto.WebshopModule.SecondHandList DTO
    /// </summary>
    public class SecondHandToSecondHand
    {
        public CompanyGroup.Dto.WebshopModule.SecondHandList Map(List<CompanyGroup.Domain.WebshopModule.SecondHand> secondHandList)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.SecondHandList()
                {
                    Items = secondHandList.ConvertAll<CompanyGroup.Dto.WebshopModule.SecondHand>(x => MapItem(x)),

                    MinimumPrice = String.Format( "{0}", secondHandList.Min( x => x.Price ) )
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.SecondHandList() { Items = new List<Dto.WebshopModule.SecondHand>(), MinimumPrice = "0" }; }
        }

        private CompanyGroup.Dto.WebshopModule.SecondHand MapItem(CompanyGroup.Domain.WebshopModule.SecondHand from)
        {
            return new CompanyGroup.Dto.WebshopModule.SecondHand()
            {
                ConfigId = from.ConfigId, 
                InventLocationId = from.InventLocationId, 
                Price = from.Price, 
                Quantity = from.Quantity, 
                StatusDescription = from.StatusDescription
            };
        }

    }


}
