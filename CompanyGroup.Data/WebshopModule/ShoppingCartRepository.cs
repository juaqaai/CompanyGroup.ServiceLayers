using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.WebshopModule
{
    /// <summary>
    /// kosár kiolvasás azonosító alapján, 
    /// kosár lista kiolvasása visitorId alapján,
    /// kosár hozzáadása kollekcióhoz,
    /// kosár elem mennyiség frissítése,
    /// kosár elem hozzáadás,
    /// kosár elem törlés
    /// </summary>
    public class ShoppingCartRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.WebshopModule.IShoppingCartRepository
    {
        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="settings"></param>
        public ShoppingCartRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// kosár kiolvasása kosárazonosító alapján 
        /// a tárolt és az új kosár elemek kerülnek az eredményhalmazba
        /// csak a létrehozott, vagy tárolt elemek lesznek a kosár elemek státuszai (Created és a Stored -re szűrés)
        /// ha nincs meg a keresett elem, akkor null-al tér vissza
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.ShoppingCart GetShoppingCart(int cartId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((cartId > 0), "The cartId parameter must be greather than zero!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetShoppingCart").SetInt32("CartId", cartId);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = query.UniqueResult<CompanyGroup.Domain.WebshopModule.ShoppingCart>();

                return shoppingCart;
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        /// <summary>
        /// kosár lista kiolvasása visitorId alapján
        /// a tárolt és az új kosár elemek kerülnek az eredményhalmazba
        /// csak a létrehozott, vagy tárolt elemek lesznek a kosár elemek státuszai (Created és a Stored -re szűrés)
        /// kosár státusz (Deleted = 0, Created = 1, Stored = 2, Posted = 3, WaitingForAutoPost = 4)
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="onlyCreatedOrStored"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollection(string visitorId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrEmpty(visitorId), "The visitorId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetShoppingCartCollection").SetString("VisitorId", visitorId);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> resultList = query.List<CompanyGroup.Domain.WebshopModule.ShoppingCart>() as List<CompanyGroup.Domain.WebshopModule.ShoppingCart>;

                return resultList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár hozzáadása kollekcióhoz, új kosárazonosítóval tér vissza
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public int Add(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((shoppingCart != null), "The shoppingCart cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartInsert").SetString("VisitorId", shoppingCart.VisitorId)
                                                                                                  .SetString("Name", shoppingCart.Name)
                                                                                                  .SetEnum("PaymentTerms", shoppingCart.PaymentTerms)
                                                                                                  .SetEnum("DeliveryTerms", shoppingCart.DeliveryTerms)
                                                                                                  .SetDateTime("DeliveryDateRequested", shoppingCart.Shipping.DateRequested)
                                                                                                  .SetString("DeliveryZipCode", shoppingCart.Shipping.ZipCode)
                                                                                                  .SetString("DeliveryCity", shoppingCart.Shipping.City)
                                                                                                  .SetString("DeliveryCountry", shoppingCart.Shipping.Country)
                                                                                                  .SetString("DeliveryStreet", shoppingCart.Shipping.Street)
                                                                                                  .SetInt64("DeliveryAddrRecId", shoppingCart.Shipping.AddrRecId)
                                                                                                  .SetBoolean("InvoiceAttached", shoppingCart.Shipping.InvoiceAttached)
                                                                                                  .SetBoolean("Active", shoppingCart.Active)
                                                                                                  .SetString("Currency", shoppingCart.Currency);
                int cartId = query.UniqueResult<int>();

                return cartId;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár eltávolítása
        /// Az "Active" mező értékének "false"-ra állításával, Státusz mező "Deleted"-re állításával
        /// </summary>
        /// <param name="cartId"></param>
        public void Remove(int cartId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((cartId > 0), "The id parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartSetStatus").SetInt32("CartId", cartId)
                                                                                                     .SetEnum("Status", CartStatus.Deleted);

                int ret = query.UniqueResult<int>();

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár feladása, WaitingForAutoPost státusz beállítás történik
        /// </summary>
        /// <param name="cartId"></param>
        public void Post(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((shoppingCart != null), "The shoppingCart cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartUpdate").SetInt32("CartId", shoppingCart.Id)
                                                                                                  .SetString("Name", shoppingCart.Name)
                                                                                                  .SetEnum("PaymentTerms", shoppingCart.PaymentTerms)
                                                                                                  .SetEnum("DeliveryTerms", shoppingCart.DeliveryTerms)
                                                                                                  .SetDateTime("DeliveryDateRequested", shoppingCart.Shipping.DateRequested)
                                                                                                  .SetString("DeliveryZipCode", shoppingCart.Shipping.ZipCode)
                                                                                                  .SetString("DeliveryCity", shoppingCart.Shipping.City)
                                                                                                  .SetString("DeliveryCountry", shoppingCart.Shipping.Country)
                                                                                                  .SetString("DeliveryStreet", shoppingCart.Shipping.Street)
                                                                                                  .SetInt64("DeliveryAddrRecId", shoppingCart.Shipping.AddrRecId)
                                                                                                  .SetBoolean("InvoiceAttached", shoppingCart.Shipping.InvoiceAttached)
                                                                                                  .SetBoolean("Active", shoppingCart.Active)
                                                                                                  .SetString("Currency", shoppingCart.Currency);
                int ret = query.UniqueResult<int>();

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// finanszírozási ajánlat feladása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="financeOffer"></param>
        //public void PostFinanceOffer(int id, CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer)
        //{
        //    try
        //    {
        //        MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

        //        IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(cartId));

        //        IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", MongoDB.Bson.BsonInt32.Create(CartStatus.FinanceOfferPosted))
        //            .Set("FinanceOffer.Address", MongoDB.Bson.BsonString.Create(financeOffer.Address))
        //            .Set("FinanceOffer.NumOfMonth", MongoDB.Bson.BsonInt32.Create(financeOffer.NumOfMonth))
        //            .Set("FinanceOffer.PersonName", MongoDB.Bson.BsonString.Create(financeOffer.PersonName))
        //            .Set("FinanceOffer.Phone", MongoDB.Bson.BsonString.Create(financeOffer.Phone))
        //            .Set("FinanceOffer.StatNumber", MongoDB.Bson.BsonString.Create(financeOffer.StatNumber));

        //        collection.Update(query, update);

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}

        /// <summary>
        /// kosár letárolása 
        /// "Status" mező értékének "Stored"-re állítása, kosár név értékének beállítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="name"></param>
        public void Store(int cartId, string name)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((cartId > 0), "The cartId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrEmpty(name), "The name parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartStore").SetInt32("CartId", cartId)
                                                                                                  .SetString("Name", name)
                                                                                                  .SetEnum("Status", CartStatus.Stored);

                int ret = query.UniqueResult<int>();

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár aktív beállítása
        /// beállítja az "CartId" alapján az "Active" mező értékét "true"-ra.
        /// beállítja az "VisitorId" alapján az "Active" mező értékét "false"-ra.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="visitorId"></param>
        public void SetActive(int cartId, string visitorId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((cartId > 0), "The cartId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrEmpty(visitorId), "The visitorId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartActivate").SetInt32("CartId", cartId)
                                                                                                    .SetString("VisitorId", visitorId);

                int ret = query.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// új visitorId beállítása a régi helyére
        /// </summary>
        /// <param name="permanentVisitorId"></param>
        /// <param name="visitorId"></param>
        public void AssociateCart(string permanentVisitorId, string visitorId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrEmpty(permanentVisitorId), "The permanentVisitorId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrEmpty(visitorId), "The visitorId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartAssociate").SetString("PermanentVisitorId", permanentVisitorId)
                                                                                                     .SetString("VisitorId", visitorId);

                int ret = query.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #region "kosár tételhez kapcsolódó műveletek"

        public CompanyGroup.Domain.WebshopModule.ShoppingCartItem GetShoppingCartLine(int lineId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetShoppingCartLine").SetInt32("LineId", lineId);

            CompanyGroup.Domain.WebshopModule.ShoppingCartItem result = query.UniqueResult<CompanyGroup.Domain.WebshopModule.ShoppingCartItem>();

            return result;
        }

        /// <summary>
        /// kosár elem mennyiség frissítése
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="quantity"></param>
        public void UpdateLineQuantity(int lineId, int quantity)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((lineId > 0), "The lineId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require((quantity > 0), "The quantity parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartLineUpdate").SetInt32("LineId", lineId)
                                                                                                      .SetInt32("Quantity", quantity);

                int ret = query.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár elem hozzáadás
        /// </summary>
        /// <param name="item"></param>
        public int AddLine(CompanyGroup.Domain.WebshopModule.ShoppingCartItem item)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((item != null), "The item cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartLineInsert").SetInt32("CartId", item.CartId)
                                                                                                      .SetString("ProductId", item.ProductId)
                                                                                                      .SetInt32("Quantity", item.Quantity)
                                                                                                      .SetInt32("Price", item.CustomerPrice)
                                                                                                      .SetString("DataAreaId", item.DataAreaId)
                                                                                                      .SetEnum("Status", item.Status);
                int lineId = query.UniqueResult<int>();

                return lineId;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///  _posts.Collection.Update(Query.EQ("_id", postId), Update.Pull("Comments", Query.EQ("_id", commentId)).Inc("TotalComments", -1));
        /// </summary>
        /// <param name="lineId"></param>
        public void RemoveLine(int lineId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((lineId > 0), "The lineId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.ShoppingCartSetLineStatus").SetInt32("LineId", lineId)
                                                                                                         .SetEnum("Status", CartItemStatus.Deleted);

                int ret = query.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                throw (ex);
            } 
        }

        #endregion
    }
}
