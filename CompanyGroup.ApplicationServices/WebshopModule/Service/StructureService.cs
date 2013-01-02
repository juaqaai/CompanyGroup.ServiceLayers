using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 IncludeExceptionDetailInFaults = true),
    //                 System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] //create instance and inject dependencies using unity container
    public class StructureService : IStructureService
    {
        private const string CACHEKEY_STRUCTURE = "structure";

        private const double CACHE_EXPIRATION_STRUCTURE = 24d;

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

        /// <summary>
        /// struktúrák lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Structures GetAll(CompanyGroup.Dto.ServiceRequest.GetAllStructure request)
        {
            request.ManufacturerIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category1IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category2IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            request.Category3IdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

            CompanyGroup.Domain.WebshopModule.StructureXml structureXml = new CompanyGroup.Domain.WebshopModule.StructureXml(new List<string>(), request.Category1IdList, request.Category2IdList, request.Category3IdList);

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

            CompanyGroup.Domain.WebshopModule.Structures structures = null;

            string cacheKey = String.Empty;

            //cache kiolvasás
            if (StructureService.StructureCacheEnabled)
            {
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_STRUCTURE, dataAreaIdCacheKey);

                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.ActionFilter, cacheKey, "ActionFilter");
                cacheKey = CompanyGroup.Helpers.ContextKeyManager.AddToKey(request.BargainFilter, cacheKey, "BargainFilter");
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
                structures = structureRepository.GetList(dataAreaId, structureXml.SerializeToXml(), request.ActionFilter, request.BargainFilter, request.IsInNewsletterFilter,
                                                         request.NewFilter, request.StockFilter, request.TextFilter, request.PriceFilter, priceFilterRelation);

                //cache-be mentés
                if (StructureService.StructureCacheEnabled)
                {
                    CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.WebshopModule.Structures>(cacheKey, structures, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_STRUCTURE)));
                }
            }

            CompanyGroup.Dto.WebshopModule.Structures result = new StructuresToStructures().Map(request.ManufacturerIdList, request.Category1IdList, request.Category2IdList, request.Category3IdList, structures);

            return result;
        }


    }
}
