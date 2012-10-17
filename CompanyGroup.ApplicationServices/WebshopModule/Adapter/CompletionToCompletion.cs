using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    ///  domain CompanyGroup.Domain.WebshopModule.SecondHand list  -> CompanyGroup.Dto.WebshopModule.SecondHandList DTO
    /// </summary>
    public class CompletionToCompletion
    {
        public CompanyGroup.Dto.WebshopModule.CompletionList Map(CompanyGroup.Domain.WebshopModule.CompletionList from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.CompletionList()
                {
                    Items = from.ConvertAll<CompanyGroup.Dto.WebshopModule.Completion>(x => MapItem(x)),
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.CompletionList() { Items = new List<Dto.WebshopModule.Completion>() }; }
        }

        private CompanyGroup.Dto.WebshopModule.Completion MapItem(CompanyGroup.Domain.WebshopModule.Completion from)
        {
            return new CompanyGroup.Dto.WebshopModule.Completion()
            {
                DataAreaId = from.DataAreaId, 
                ProductId = from.ProductId, 
                ProductName = from.ProductName, 
                RecId = from.RecId
            };
        }

    }


}
