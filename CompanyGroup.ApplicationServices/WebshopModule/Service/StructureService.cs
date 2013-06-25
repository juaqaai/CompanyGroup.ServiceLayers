using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using CompanyGroup.Helpers;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// struktúra szolgáltatás
    /// </summary>
    public class StructureService : IStructureService
    {
        private const string CACHEKEY_STRUCTURE = "structure";

        /// <summary>
        /// struktúra cache időtartama percben
        /// </summary>
        private const double CACHE_EXPIRATION_STRUCTURE = 60d;

        private static readonly bool StructureCacheEnabled = Helpers.ConfigSettingsParser.GetBoolean("StructureCacheEnabled", true);

        private CompanyGroup.Domain.WebshopModule.IStructureRepository structureRepository;

        public StructureService(CompanyGroup.Domain.WebshopModule.IStructureRepository structureRepository)
        {
            if (structureRepository == null)
            {
                throw new ArgumentNullException("StructureRepository");
            }

            this.structureRepository = structureRepository;
        }

        private string CreateTextFilterCondition(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return String.Empty;
            }

            System.Text.StringBuilder textFilterCondition = new System.Text.StringBuilder();

            string[] textFilterList = System.Text.RegularExpressions.Regex.Split(value, @"\s+");

            for (int index = 0; index < textFilterList.Length; index++)
            {
                textFilterCondition.Append("\"");

                textFilterCondition.Append(textFilterList[index]);

                textFilterCondition.Append("*");

                textFilterCondition.Append("\"");

                if (index < textFilterList.Length - 1)
                {
                    textFilterCondition.Append(" AND ");
                }
            }

            return textFilterCondition.ToString();
        }

        /// <summary>
        /// struktúrák lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Structures GetAll(CompanyGroup.Dto.WebshopModule.GetAllStructureRequest request)
        {
            try
            {
                request.ManufacturerIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                string dataAreaId = String.Empty;

                string dataAreaIdCacheKey = String.Empty;

                if (request.HrpFilter && !request.BscFilter)
                {
                    dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp;

                    dataAreaIdCacheKey = CompanyGroup.Domain.Core.Constants.DataAreaIdHrp;
                }
                else if (request.BscFilter && !request.HrpFilter)
                {
                    dataAreaId = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc;

                    dataAreaIdCacheKey = CompanyGroup.Domain.Core.Constants.DataAreaIdBsc;
                }
                else
                {
                    dataAreaIdCacheKey = "all";
                }

                //szűrés ár értékre
                int priceFilterRelation = 0;

                int.TryParse(request.PriceFilterRelation, out priceFilterRelation);

                //szöveges kereső paraméter FULL TEXT SEARCH paraméterré alakítása "ASUS*" AND "számítógép*"
                string textFilter = CreateTextFilterCondition(request.TextFilter);

                CompanyGroup.Domain.WebshopModule.Structures structures = null;

                string cacheKey = String.Empty;

                //cache kiolvasás
                if (StructureService.StructureCacheEnabled)
                {
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_STRUCTURE, dataAreaIdCacheKey);

                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.DiscountFilter, cacheKey, "DiscountFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.SecondhandFilter, cacheKey, "SecondhandFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.IsInNewsletterFilter, cacheKey, "IsInNewsletterFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.NewFilter, cacheKey, "NewFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.StockFilter, cacheKey, "StockFilter");
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.TextFilter), cacheKey, request.TextFilter);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilter), cacheKey, request.PriceFilter);
                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(!String.IsNullOrWhiteSpace(request.PriceFilterRelation), cacheKey, request.PriceFilterRelation);

                    structures = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.WebshopModule.Structures>(cacheKey);
                }

                //vagy nem engedélyezett a cache, vagy nem volt a cache-ben
                if (structures == null)
                {
                    structures = structureRepository.GetList(dataAreaId,
                                                             String.Empty,  // ConvertData.ConvertStringListToDelimitedString(request.ManufacturerIdList)
                                                             String.Empty,  // ConvertData.ConvertStringListToDelimitedString(request.Category1IdList)
                                                             String.Empty,  // ConvertData.ConvertStringListToDelimitedString(request.Category2IdList)
                                                             String.Empty,  // ConvertData.ConvertStringListToDelimitedString(request.Category3IdList)
                                                             request.DiscountFilter,
                                                             request.SecondhandFilter,
                                                             request.IsInNewsletterFilter,
                                                             request.NewFilter,
                                                             request.StockFilter,
                                                             textFilter,
                                                             request.PriceFilter,
                                                             priceFilterRelation);

                    //cache-be mentés
                    if (StructureService.StructureCacheEnabled)
                    {
                        CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.Structures>(cacheKey, structures, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_STRUCTURE)));
                    }
                }

                CompanyGroup.Dto.WebshopModule.Structures result = new StructuresToStructures().Map(request.ManufacturerIdList, request.Category1IdList, request.Category2IdList, request.Category3IdList, structures);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
