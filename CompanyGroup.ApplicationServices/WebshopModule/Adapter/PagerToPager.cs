using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class PagerToPager
    {
        /// <summary>
        /// domain CompanyGroup.Domain.WebshopModule.Pager -> DTO CompanyGroup.Dto.WebshopModule.Pager 
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Pager Map(CompanyGroup.Domain.WebshopModule.Pager pager, int itemsOnPage)
        {
            CompanyGroup.Dto.WebshopModule.Pager result = new CompanyGroup.Dto.WebshopModule.Pager();

            result.FirstEnabled = pager.FirstEnabled;

            result.LastEnabled = pager.LastEnabled;

            result.LastPageIndex = pager.LastPageIndex;

            result.NextEnabled = pager.NextEnabled;

            result.PageItemList = pager.PageItemList.ConvertAll(x => new CompanyGroup.Dto.WebshopModule.PageItem() { Index = x.Index, Selected = x.Selected, Value = x.Value });

            result.PreviousEnabled = pager.PreviousEnabled;

            //elemek száma az oldalon
            result.VisibleItemList = new List<CompanyGroup.Dto.WebshopModule.VisibleItem>();

            for (int i = 1; i <= 10; i++)
            {
                result.VisibleItemList.Add(new CompanyGroup.Dto.WebshopModule.VisibleItem() { Value = i * 10, Selected = itemsOnPage.Equals(i * 10) });
            }

            return result;
        }
    }
}
