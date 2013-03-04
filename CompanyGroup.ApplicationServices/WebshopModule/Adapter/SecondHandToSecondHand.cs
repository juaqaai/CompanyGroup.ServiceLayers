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
                CompanyGroup.Domain.WebshopModule.SecondHand minimumPrice = secondHandList.OrderBy( x => x.Price ).FirstOrDefault();

                return new CompanyGroup.Dto.WebshopModule.SecondHandList()
                {
                    Items = secondHandList.ConvertAll<CompanyGroup.Dto.WebshopModule.SecondHand>(x => MapItem(x)),

                    MinimumPrice = String.Format("{0}", minimumPrice.CustomerPrice)
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.SecondHandList() { Items = new List<Dto.WebshopModule.SecondHand>(), MinimumPrice = "0" }; }
        }

        /// <summary>
        /// domain CompanyGroup.Domain.WebshopModule.SecondHand -> CompanyGroup.Dto.WebshopModule.SecondHand DTO. 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        private CompanyGroup.Dto.WebshopModule.SecondHand MapItem(CompanyGroup.Domain.WebshopModule.SecondHand from)
        {
            return new CompanyGroup.Dto.WebshopModule.SecondHand()
            {
                ConfigId = from.ConfigId, 
                InventLocationId = from.InventLocationId,
                Price = String.Format("{0}", from.CustomerPrice),
                Quantity = from.Quantity, 
                StatusDescription = from.StatusDescription, 
                DataAreaId = from.DataAreaId,
                Id = from.Id,
                ProductId = from.ProductId
            };
        }

    }


}
