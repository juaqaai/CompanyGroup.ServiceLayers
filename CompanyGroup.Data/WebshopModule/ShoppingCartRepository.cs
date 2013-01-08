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
        /// konstruktor mongoDb beállításokkal
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
        public CompanyGroup.Domain.WebshopModule.ShoppingCart GetCart(int id)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((id > 0), "The cartId parameter must be greather than zero!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetCart").SetInt32("Id", id);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = query.UniqueResult<CompanyGroup.Domain.WebshopModule.ShoppingCart>();

                return shoppingCart;
            }
            catch(Exception ex)
            {
                throw(ex);
            }
        }

        public List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollection(string visitorId)
        {
            return GetCartCollection(visitorId, false);
        }

        /// <summary>
        /// kosár lista kiolvasása visitorId alapján
        /// a tárolt és az új kosár elemek kerülnek az eredményhalmazba
        /// csak a létrehozott, vagy tárolt elemek lesznek a kosár elemek státuszai (Created és a Stored -re szűrés)
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="onlyStored"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollection(string visitorId, bool onlyStored)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrEmpty(visitorId), "The visitorId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetCartCollection").SetString("VisitorId", visitorId).SetBoolean("OnlyStored", onlyStored);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> resultList = query.List<CompanyGroup.Domain.WebshopModule.ShoppingCart>() as List<CompanyGroup.Domain.WebshopModule.ShoppingCart>;

                return resultList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public CompanyGroup.Domain.WebshopModule.ShoppingCartItem GetShoppingCartItem(int id)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.GetShoppingCartItem").SetInt32("Id", id);

            CompanyGroup.Domain.WebshopModule.ShoppingCartItem result = query.UniqueResult<CompanyGroup.Domain.WebshopModule.ShoppingCartItem>();

            return result;
        }

        /// <summary>
        /// kosár hozzáadása kollekcióhoz
        /// </summary>
        /// <param name="shoppingCartCollection"></param>
        public void Add(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart)
        {
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //collection.Insert(shoppingCart);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár eltávolítása kollekcióból
        /// Az "Active" mező értékének "false"-ra állításával
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            CompanyGroup.Domain.Utils.Check.Require(!id.Equals(MongoDB.Bson.ObjectId.Empty), "The id parameter cannot be null!");
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", id);

                //IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", CartStatus.Deleted).Set("Active", false);

                //collection.Update(query, update);

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
        public void Post(int id, PaymentTerms paymentTerms, DeliveryTerms deliveryTerms, CompanyGroup.Domain.WebshopModule.Shipping shipping)
        {
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(cartId));

                //IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", MongoDB.Bson.BsonInt32.Create(CartStatus.WaitingForAutoPost))
                //    .Set("PaymentTerms", MongoDB.Bson.BsonInt32.Create(paymentTerms))
                //    .Set("DeliveryTerms", MongoDB.Bson.BsonInt32.Create(deliveryTerms))
                //    .Set("Shipping.AddrRecId", MongoDB.Bson.BsonInt64.Create(shipping.AddrRecId))
                //    .Set("Shipping.City", MongoDB.Bson.BsonString.Create(shipping.City))
                //    .Set("Shipping.DateRequested", MongoDB.Bson.BsonDateTime.Create(shipping.DateRequested))
                //    .Set("Shipping.InvoiceAttached", MongoDB.Bson.BsonBoolean.Create(shipping.InvoiceAttached))
                //    .Set("Shipping.Street", MongoDB.Bson.BsonString.Create(shipping.Street))
                //    .Set("Shipping.ZipCode", MongoDB.Bson.BsonString.Create(shipping.ZipCode));

                //collection.Update(query, update);

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
        /// kosár letárolása későbbi felhasználásra
        /// "Status" mező értékének "Stored"-re állítása, név értékének beállítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="name"></param>
        public void Store(int id, string name)
        {
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", cartId);

                //IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", CartStatus.Stored).Set("Name", name);

                //collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár aktív / inaktív beállítása
        /// beállítja az "id" alapján az "Active" mező értékét "false"-ra, vagy "true"-ra.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        public void SetActive(int id, bool active)
        {
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", cartId);

                //IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Active", active);

                //collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// létezik-e a productId-vel rendelkező termék a cartId azonosítójú kosárban?
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool ExistsProductInCart(int id, string productId) 
        {
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", id),
                //                                                      MongoDB.Driver.Builders.Query.EQ("Items.ProductId", productId));

                //return collection.Count(query) > 0;
                return false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// új visitorId beállítása a régi helyére
        /// </summary>
        /// <param name="id"></param>
        /// <param name="visitorId"></param>
        public void AssociateCart(int id, string visitorId)
        {
            try
            {
            //    MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

            //    IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", id),
            //                                                          MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(CartStatus.Stored)));

            //    IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("VisitorId", visitorId);

            //    collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #region "kosár tételhez kapcsolódó műveletek"

        /*
        _posts.Collection.Update(
        Query.EQ("_id", post.PostId),
        Update.Set("Title", post.Title)
            .Set("Url", post.Url)
            .Set("Summary", post.Summary)
            .Set("Details", post.Details));  
         * 
        QueryComplete query = Query.EQ("companies._id", 
            BsonValue.Create(new Guid("0AE91D6B-A8FA-4D0D-A94A-91D6AC9EE343")));
        var update = Update.Set("companies.$.companynotes", "companynotes");
        personCollection.Update(query, update, UpdateFlags.Multi, SafeMode.True);
         * 
        var update = MongoDB.Driver.Builders.Update.Set("companies.$.name", "GreenfinNewName");         
         */
        /// <summary>
        /// kosár elem mennyiség frissítése
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        public void UpdateLineQuantity(int id, int quantity)
        {
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", cartId),
                //                                                      MongoDB.Driver.Builders.Query.EQ("Items.ProductId", productId));

                //IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Items.$.Quantity", quantity);

                //collection.Update(query, update);

                //collection.Update(MongoDB.Driver.Builders.Query.EQ("Id", product.Id), MongoDB.Driver.Builders.Update.Pull("Pictures", MongoDB.Driver.Builders.Query.EQ("FileName", MongoDB.Bson.BsonValue.Create(picture.FileName))));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár elem hozzáadás
        ///     _posts.Collection.Update(Query.EQ("_id", postId), Update.PushWrapped("Comments", comment).Inc("TotalComments", 1))
        /// </summary>
        /// <param name="item"></param>
        public void AddLine(CompanyGroup.Domain.WebshopModule.ShoppingCartItem item)
        {
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", item.Id);

                //collection.Update(query, MongoDB.Driver.Builders.Update.PushWrapped("Items", item));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///  _posts.Collection.Update(Query.EQ("_id", postId), Update.Pull("Comments", Query.EQ("_id", commentId)).Inc("TotalComments", -1));
        /// </summary>
        /// <param name="id"></param>
        public void RemoveLine(int id)
        {
            try
            {
                //MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                //IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", cartId),
                //                                      MongoDB.Driver.Builders.Query.EQ("Items.ProductId", productId));

                //IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Items.$.Status", CartItemStatus.Deleted);

                //collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            } 
        }

        #endregion
    }
}
