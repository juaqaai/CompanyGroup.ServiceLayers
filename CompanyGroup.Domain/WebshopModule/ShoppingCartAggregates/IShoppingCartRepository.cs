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
        CompanyGroup.Domain.WebshopModule.ShoppingCart GetCartByKey(string cartId);

        /// <summary>
        /// kosár kiolvasása kosár azonosító alapján
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.ShoppingCart GetCartByKey(MongoDB.Bson.ObjectId cartId);

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
        List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollectionByVisitor(string visitorId);

        /// <summary>
        /// kosár lista kiolvasása visitorId alapján
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="onlyStored"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollectionByVisitor(string visitorId, bool onlyStored);

        /// <summary>
        /// kosár eltávolítása kollekcióból
        /// </summary>
        /// <param name="cartId"></param>
        void Remove(string cartId);

        /// <summary>
        /// kosár eltávolítása kollekcióból
        /// </summary>
        /// <param name="cartId"></param>
        void Remove(MongoDB.Bson.ObjectId cartId);

        /// <summary>
        /// kosár feladás,
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="paymentTerms"></param>
        /// <param name="deliveryTerms"></param>
        /// <param name="shipping"></param>
        void Post(string cartId, PaymentTerms paymentTerms, DeliveryTerms deliveryTerms, CompanyGroup.Domain.WebshopModule.Shipping shipping);

        /// <summary>
        /// kosár mentése
        /// </summary>
        /// <param name="cartId"></param> 
        /// <param name="name"></param>
        void Store(string cartId, string name);

        /// <summary>
        /// kosár mentése
        /// </summary>
        /// <param name="cartId"></param> 
        /// <param name="name"></param>
        void Store(MongoDB.Bson.ObjectId cartId, string name);

        /// <summary>
        /// kosár aktív / inaktív beállítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="active"></param>
        void SetActive(string cartId, bool active);

        /// <summary>
        /// kosár aktív / inaktív beállítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="active"></param>
        void SetActive(MongoDB.Bson.ObjectId cartId, bool active);

        /// <summary>
        /// létezik-e az adott termék a kosárban?
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool ExistsProductInCart(string cartId, string productId);

        /// <summary>
        /// létezik-e az adott termék a kosárban?
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool ExistsProductInCart(MongoDB.Bson.ObjectId cartId, string productId);

        /// <summary>
        /// visitorId hozzárendelése a kosárhoz 
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="visitorId"></param>
        void AssociateCart(string cartId, string visitorId);

        /// <summary>
        /// visitorId hozzárendelése a kosárhoz 
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="visitorId"></param>
        void AssociateCart(MongoDB.Bson.ObjectId cartId, string visitorId);

        /// <summary>
        /// kosár elem mennyiség módosítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        void UpdateLineQuantity(string cartId, string productId, int quantity);

        /// <summary>
        /// kosár elem mennyiség módosítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        void UpdateLineQuantity(MongoDB.Bson.ObjectId cartId, string productId, int quantity);

        /// <summary>
        /// kosár elem hozzáadás
        /// </summary>
        /// <param name="item"></param>
        void AddLine(CompanyGroup.Domain.WebshopModule.ShoppingCartItem item);

        /// <summary>
        ///  kosár elem eltávolítás
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        void RemoveLine(string cartId, string productId);

        /// <summary>
        ///  kosár elem eltávolítás
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        void RemoveLine(MongoDB.Bson.ObjectId cartId, string productId);

        /// <summary>
        /// finanszírozási ajánlat elküldés
        /// </summary>
        /// <param name="cartId"></param>
        void PostFinanceOffer(string cartId, CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer);

        /// <summary>
        /// 
        /// </summary>
        void Disconnect();
    }
}
