using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CompanyGroup.GlobalServices
{
    [ServiceBehavior(UseSynchronizationContext = false,
                     InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true),
                     System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    [CompanyGroup.GlobalServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] 
    public class ProductService : ServiceBase, IProductService
    {

        /// <summary>
        /// konstruktor
        /// </summary>
        //public ProductService() : base()
        //{
        //}

        /// <summary>
        /// összes, a feltételeknek megfelelő termékelem leválogatása 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.GlobalServices.Dto.GetAllResponse GetAll(CompanyGroup.GlobalServices.Dto.GetAllRequest request)
        {
            long count = 0;

            request.ManufacturerIdList.RemoveAll(x => String.IsNullOrEmpty(x));

            request.Category1IdList.RemoveAll(x => String.IsNullOrEmpty(x));

            request.Category2IdList.RemoveAll(x => String.IsNullOrEmpty(x));

            request.Category3IdList.RemoveAll(x => String.IsNullOrEmpty(x));

            CompanyGroup.Dto.ServiceRequest.GetAllProduct productFilter = new CompanyGroup.Dto.ServiceRequest.GetAllProduct()
            {
                ActionFilter = request.ActionFilter,
                BargainFilter = false,
                Category1IdList = request.Category1IdList,
                Category2IdList = request.Category2IdList,
                Category3IdList = request.Category3IdList,
                CurrentPageIndex = request.CurrentPageIndex,
                BscFilter = request.DataAreaId.Equals("bsc"),
                HrpFilter = request.DataAreaId.Equals("hrp"),
                ItemsOnPage = request.ItemsOnPage,
                ManufacturerIdList = request.ManufacturerIdList,
                NewFilter = false,
                Sequence = 2,
                StockFilter = request.StockFilter,
                TextFilter = String.Empty,
                VisitorId = String.Empty
            };

            CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.WebshopModule.Products>("ProductService", "GetAll", productFilter);


            //CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

            //érvényes belépés esetén ár kalkulálása 

            products.ListCount = count;

            return new CompanyGroup.GlobalServices.Dto.GetAllResponse() { Products = new CompanyGroup.GlobalServices.Adapter.ProductsToProducts().Map(products), ListCount = products.ListCount };
        }

        /// <summary>
        /// productId és dataAreaId összetett kulccsal rendelkező termékelem lekérdezése
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public CompanyGroup.GlobalServices.Dto.GetItemByProductIdResponse GetItemByProductId(CompanyGroup.GlobalServices.Dto.GetItemByProductIdRequest request)
        {

            CompanyGroup.Dto.ServiceRequest.GetItemByProductId productItemFilter = new CompanyGroup.Dto.ServiceRequest.GetItemByProductId()
            {
                DataAreaId = request.DataAreaId,
                ProductId = request.ProductId,
                VisitorId = String.Empty
            };

            CompanyGroup.Dto.WebshopModule.Product response = this.PostJSonData<CompanyGroup.Dto.WebshopModule.Product>("ProductService", "GetItemByProductId", productItemFilter);

            CompanyGroup.GlobalServices.Dto.Product product = new CompanyGroup.GlobalServices.Adapter.ProductToProduct().Map(response);

            return new CompanyGroup.GlobalServices.Dto.GetItemByProductIdResponse() { Product = product };
        }

    }
}
