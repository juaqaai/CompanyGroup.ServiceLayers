using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IShoppingCartRepository
    {
        /// <summary>
        /// kosár kiolvasása kosár azonosító alapján 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.ShoppingCart GetCart(int id);

        /// <summary>
        /// kosár hozzáadása kollekcióhoz
        /// </summary>
        /// <param name="shoppingCart"></param>
        void Add(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart);

        /// <summary>
        /// kosár lista kiolvasása visitorId alapján
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollection(string visitorId);

        /// <summary>
        /// kosár lista kiolvasása visitorId alapján
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="onlyStored"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollection(string visitorId, bool onlyStored);

        /// <summary>
        /// kosár eltávolítása kollekcióból
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);

        /// <summary>
        /// kosár feladás,
        /// </summary>
        /// <param name="id"></param>
        /// <param name="paymentTerms"></param>
        /// <param name="deliveryTerms"></param>
        /// <param name="shipping"></param>
        void Post(int id, PaymentTerms paymentTerms, DeliveryTerms deliveryTerms, CompanyGroup.Domain.WebshopModule.Shipping shipping);

        /// <summary>
        /// kosár mentése
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="name"></param>
        void Store(int id, string name);

        /// <summary>
        /// kosár aktív / inaktív beállítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        void SetActive(int id, bool active);

        /// <summary>
        /// létezik-e az adott termék a kosárban?
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool ExistsProductInCart(int id, string productId);

        /// <summary>
        /// visitorId hozzárendelése a kosárhoz 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="visitorId"></param>
        void AssociateCart(int id, string visitorId);

        /// <summary>
        /// kosár elem mennyiség módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        void UpdateLineQuantity(int id, int quantity);

        /// <summary>
        /// kosár elem hozzáadás
        /// </summary>
        /// <param name="item"></param>
        void AddLine(CompanyGroup.Domain.WebshopModule.ShoppingCartItem item);

        /// <summary>
        ///  kosár elem eltávolítás
        /// </summary>
        /// <param name="id"></param>
        void RemoveLine(int id);

        /// <summary>
        /// finanszírozási ajánlat elküldés
        /// </summary>
        /// <param name="id"></param>
        //void PostFinanceOffer(int id, CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer);
    }
}
