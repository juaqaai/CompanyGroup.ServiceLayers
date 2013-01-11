using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IShoppingCartRepository
    {
        /// <summary>
        /// kosár kiolvasása kosár azonosító alapján 
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.ShoppingCart GetShoppingCart(int cartId);

        /// <summary>
        /// kosár lista kiolvasása visitorId alapján
        /// </summary>
        /// <param name="visitorId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollection(string visitorId);

        /// <summary>
        /// kosár hozzáadása kollekcióhoz
        /// </summary>
        /// <param name="shoppingCart"></param>
        int Add(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart);

        /// <summary>
        /// kosár eltávolítása kollekcióból
        /// </summary>
        /// <param name="cartId"></param>
        void Remove(int cartId);

        /// <summary>
        /// kosár feladás,
        /// </summary>
        void Post(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart);

        /// <summary>
        /// kosár mentése
        /// </summary>
        /// <param name="cartId"></param> 
        /// <param name="name"></param>
        void Store(int cartId, string name);

        /// <summary>
        /// kosár aktív beállítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="visitorId"></param>
        void SetActive(int cartId, string visitorId);

        /// <summary>
        /// létezik-e az adott termék a kosárban? (szervizrétegbe költözött)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        //bool ExistsProductInCart(int id, string productId);

        /// <summary>
        /// visitorId hozzárendelése a kosárhoz 
        /// </summary>
        /// <param name="permanentVisitorId"></param>
        /// <param name="visitorId"></param>
        void AssociateCart(string permanentVisitorId, string visitorId);

        /// <summary>
        /// kosár elem mennyiség módosítása
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="quantity"></param>
        void UpdateLineQuantity(int lineId, int quantity);

        /// <summary>
        /// kosár elem hozzáadás
        /// </summary>
        /// <param name="item"></param>
        int AddLine(CompanyGroup.Domain.WebshopModule.ShoppingCartItem item);

        /// <summary>
        ///  kosár elem eltávolítás
        /// </summary>
        /// <param name="lineId"></param>
        void RemoveLine(int lineId);

        /// <summary>
        /// finanszírozási ajánlat elküldés
        /// </summary>
        /// <param name="id"></param>
        //void PostFinanceOffer(int id, CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer);
    }
}
