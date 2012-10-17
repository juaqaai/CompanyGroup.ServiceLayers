using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// bevásárlókosárhoz tartozó application szerviz interfészek
    /// </summary>
    [ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.WebShopModule/", Name = "ShoppingCartService")]
    public interface IShoppingCartService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract(Action = "AssociateCart")]
        [WebInvoke(Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AssociateCart(CompanyGroup.Dto.ServiceRequest.AssociateCart request);

        /// <summary>
        /// bevásárlókosár hozzáadása bevásárlókosár kollekcióhoz, 
        /// új kosár inicializálása + új elem hozzáadása
        /// </summary>
        [OperationContract(Action = "AddCart")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/AddCart",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartInfo AddCart(CompanyGroup.Dto.ServiceRequest.AddCart request);

        /// <summary>
        /// kosár mentése
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "SaveCart")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/SaveCart",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartInfo SaveCart(CompanyGroup.Dto.ServiceRequest.SaveCart request);

        /// <summary>
        /// teljes kosár törlése meglévő kollekcióból 
        /// </summary>
        /// <param name="cartId"></param>
        [OperationContract(Action = "RemoveCart")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/RemoveCart",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartInfo RemoveCart(CompanyGroup.Dto.ServiceRequest.RemoveCart request);

        /// <summary>
        /// aktiv kosár beállítása   
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [OperationContract(Action = "ActivateCart")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/ActivateCart",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartInfo ActivateCart(CompanyGroup.Dto.ServiceRequest.ActivateCart request);

        [OperationContract(Action = "CreateFinanceOffer")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/CreateFinanceOffer",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment CreateFinanceOffer(CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer request);

        [OperationContract(Action = "CreateOrder")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/CreateOrder",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.OrderFulFillment CreateOrder(CompanyGroup.Dto.ServiceRequest.SalesOrderCreate request);

        /// <summary>
        /// új elem hozzáadása meglévő kosárhoz
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "AddLine")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/AddLine",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions AddLine(CompanyGroup.Dto.ServiceRequest.AddLine request);

        /// <summary>
        /// elem törlése meglévő kosárból
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "RemoveLine")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/RemoveItem",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions RemoveLine(CompanyGroup.Dto.ServiceRequest.RemoveLine request);

        /// <summary>
        /// meglévő kosárban elem frissítése 
        /// </summary>
        /// <param name="request"></param>
        [OperationContract(Action = "UpdateLineQuantity")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/UpdateItemQuantity",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartAndLeasingOptions UpdateLineQuantity(CompanyGroup.Dto.ServiceRequest.UpdateLineQuantity request);

        /// <summary>
        /// felhasználóhoz tartozó aktív kosár kiolvasása
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetCartCollectionByVisitor")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/GetItemsByVisitorId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartCollection GetCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor request);

        /// <summary>
        /// aktiv kosár kiolvasása   
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetActiveCart")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/GetActiveCart",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetActiveCart(CompanyGroup.Dto.ServiceRequest.GetActiveCart request);

        /// <summary>
        /// céghez, azon belül személyhez tartozó érvényes / tárolt kosarak kiolvasása
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetStoredOpenedShoppingCartCollectionByVisitor")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/GetItemsByVisitorId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.StoredOpenedShoppingCartCollection GetStoredOpenedShoppingCartCollectionByVisitor(CompanyGroup.Dto.ServiceRequest.GetCartCollectionByVisitor request);

        /// <summary>
        /// kosárazonosítóval rendelkező kosár kiolvasása   
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [OperationContract(Action = "GetCartByKey")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/GetCartByKey",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCart GetCartByKey(CompanyGroup.Dto.ServiceRequest.GetCartByKey request);

        [OperationContract(Action = "GetShoppingCartInfo")]
        [WebInvoke(Method = "POST",
            //UriTemplate = "/GetShoppingCartInfo",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CompanyGroup.Dto.WebshopModule.ShoppingCartInfo GetShoppingCartInfo(CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo request);


    }
}
