using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    ///  domain CompanyGroup.Domain.WebshopModule.CatalogueDetailsLog list  -> CompanyGroup.Dto.WebshopModule.CatalogueDetailsLog DTO
    /// </summary>
    public class CatalogueDetailsLogToCatalogueDetailsLog
    {
        /// <summary>
        /// domain CatalogueDetailsLog -> DTO CatalogueDetailsLog
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogList Map(List<CompanyGroup.Domain.WebshopModule.CatalogueDetailsLog> from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogList()
                {
                    Items = from.ConvertAll<CompanyGroup.Dto.WebshopModule.CatalogueDetailsLog>(x => MapItem(x)),
                };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.CatalogueDetailsLogList() { Items = new List<Dto.WebshopModule.CatalogueDetailsLog>() }; }
        }

        /// <summary>
        /// Domain Completion -> DTO Completion
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        private CompanyGroup.Dto.WebshopModule.CatalogueDetailsLog MapItem(CompanyGroup.Domain.WebshopModule.CatalogueDetailsLog from)
        {
            return new CompanyGroup.Dto.WebshopModule.CatalogueDetailsLog()
            {
                DataAreaId = from.DataAreaId, 
                ProductId = from.ProductId, 
                ProductName = from.ProductName,
                PictureId = from.PictureId, 
                EnglishProductName = from.EnglishProductName
            };
        }

    }


}
