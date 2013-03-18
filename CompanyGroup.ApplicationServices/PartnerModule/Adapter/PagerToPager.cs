using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class PagerToPager
    {
        /// <summary>
        /// domain CompanyGroup.Domain.WebshopModule.Pager -> DTO CompanyGroup.Dto.WebshopModule.Pager 
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Pager Map(CompanyGroup.Domain.PartnerModule.Pager pager, int itemsOnPage)
        {
            CompanyGroup.Dto.PartnerModule.Pager result = new CompanyGroup.Dto.PartnerModule.Pager();

            result.FirstEnabled = pager.FirstEnabled;

            result.LastEnabled = pager.LastEnabled;

            result.LastPageIndex = pager.LastPageIndex;

            result.NextEnabled = pager.NextEnabled;

            result.PageItemList = pager.PageItemList.ConvertAll(x => new CompanyGroup.Dto.PartnerModule.PageItem() { Index = x.Index, Selected = x.Selected, Value = x.Value });

            result.PreviousEnabled = pager.PreviousEnabled;

            CompanyGroup.Domain.PartnerModule.PageItem pageItem = pager.PageItemList.Find(x => x.Selected);

            result.NextPageIndex = (pageItem != null) ? ( (pager.NextEnabled) ? pageItem.Index + 1 : pager.LastPageIndex ) : 1;

            result.PreviousPageIndex = (pageItem != null) ? ( (pager.PreviousEnabled) ? pageItem.Index - 1 : 1) : 1;

            //elemek száma az oldalon
            result.VisibleItemList = new List<CompanyGroup.Dto.PartnerModule.VisibleItem>();

            for (int i = 1; i <= 10; i++)
            {
                result.VisibleItemList.Add(new CompanyGroup.Dto.PartnerModule.VisibleItem() { Value = i * 10, Selected = itemsOnPage.Equals(i * 10) });
            }

            return result;
        }
    }
}
