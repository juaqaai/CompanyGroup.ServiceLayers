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
    public class ShoppingCartRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.WebshopModule.ShoppingCart>, CompanyGroup.Domain.WebshopModule.IShoppingCartRepository
    {
        /// <summary>
        /// kosár kollekció neve mongoDB-ben
        /// </summary>
        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("ShoppingCartCollectionName", "ShoppingCart");

        /// <summary>
        /// konstruktor mongoDb beállításokkal
        /// </summary>
        /// <param name="settings"></param>
        public ShoppingCartRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        /// <summary>
        /// kosár kiolvasása kosárazonosító alapján 
        /// a tárolt és az új kosár elemek kerülnek az eredményhalmazba
        /// csak a létrehozott, vagy tárolt elemek lesznek a kosár elemek státuszai (Created és a Stored -re szűrés)
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.ShoppingCart GetCartByKey(string cartId)
        {
            return this.GetCartByKey(this.ConvertStringToBsonObjectId(cartId));
        }

        /// <summary>
        /// kosár kiolvasása kosárazonosító alapján 
        /// a tárolt és az új kosár elemek kerülnek az eredményhalmazba
        /// csak a létrehozott, vagy tárolt elemek lesznek a kosár elemek státuszai (Created és a Stored -re szűrés)
        /// ha nincs meg a keresett elem, akkor null-al tér vissza
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.ShoppingCart GetCartByKey(MongoDB.Bson.ObjectId cartId)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require((MongoDB.Bson.ObjectId.Empty != cartId), "The cartId parameter cannot be null or empty!");

                MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query1 = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", cartId),
                                                                       MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CartStatus.Created))));

                IMongoQuery query2 = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", cartId),
                                                                       MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CartStatus.Stored))));

                IMongoQuery query = MongoDB.Driver.Builders.Query.Or(query1, query2);

                CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart = collection.FindOne(query);

                if (shoppingCart != null)
                { 
                    shoppingCart.Items.RemoveAll(x => x.Status.Equals(CartItemStatus.Deleted) || x.Status.Equals(CartItemStatus.Posted));
                }

                return shoppingCart;
            }
            catch(Exception ex)
            {
                throw(ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        public List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollectionByVisitor(string visitorId)
        { 
            return GetCartCollectionByVisitor(visitorId, false);
        }

        /// <summary>
        /// kosár lista kiolvasása visitorId alapján
        /// a tárolt és az új kosár elemek kerülnek az eredményhalmazba
        /// csak a létrehozott, vagy tárolt elemek lesznek a kosár elemek státuszai (Created és a Stored -re szűrés)
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="onlyStored"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.ShoppingCart> GetCartCollectionByVisitor(string visitorId, bool onlyStored)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!visitorId.Equals(MongoDB.Bson.ObjectId.Empty), "The visitorId parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                MongoDB.Driver.IMongoQuery query;

                if (onlyStored)
                {
                    query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("VisitorId", visitorId),
                                                              MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(Convert.ToInt32(CartStatus.Stored))));

                }
                else
                {
                    query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("VisitorId", visitorId),
                                                              MongoDB.Driver.Builders.Query.In("Status", MongoDB.Bson.BsonArray.Create(new List<int>() { Convert.ToInt32(CartStatus.Created), Convert.ToInt32(CartStatus.Stored) })));                
                }


                //IMongoQuery query1 = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("VisitorId", visitorId),
                //                                                       MongoDB.Driver.Builders.Query.EQ("Status", Convert.ToInt32(CartStatus.Created)));

                //IMongoQuery query2 = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("VisitorId", visitorId),
                //                                                       MongoDB.Driver.Builders.Query.EQ("Status", Convert.ToInt32(CartStatus.Stored)));

                //IMongoQuery query = MongoDB.Driver.Builders.Query.Or(query1, query2);

                MongoCursor<CompanyGroup.Domain.WebshopModule.ShoppingCart> shoppingCarts = collection.Find(query);

                List<CompanyGroup.Domain.WebshopModule.ShoppingCart> resultList = new List<CompanyGroup.Domain.WebshopModule.ShoppingCart>();

                foreach(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart in shoppingCarts)
                {
                    shoppingCart.Items.RemoveAll(x => x.Status.Equals(CartItemStatus.Deleted) || x.Status.Equals(CartItemStatus.Posted));

                    resultList.Add(shoppingCart);
                }

                return resultList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// kosár hozzáadása kollekcióhoz
        /// </summary>
        /// <param name="shoppingCartCollection"></param>
        public void Add(CompanyGroup.Domain.WebshopModule.ShoppingCart shoppingCart)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                collection.Insert(shoppingCart);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// kosár eltávolítása kollekcióból
        /// Az "Active" mező értékének "false"-ra állításával
        /// </summary>
        /// <param name="id"></param>
        public void Remove(string id)
        {
            Remove(this.ConvertStringToBsonObjectId(id));
        }

        /// <summary>
        /// kosár eltávolítása kollekcióból
        /// Az "Active" mező értékének "false"-ra állításával
        /// </summary>
        /// <param name="id"></param>
        public void Remove(MongoDB.Bson.ObjectId id)
        {
            CompanyGroup.Domain.Utils.Check.Require(!id.Equals(MongoDB.Bson.ObjectId.Empty), "The id parameter cannot be null!");
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", id);

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", CartStatus.Deleted).Set("Active", false);

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// kosár feladása, WaitingForAutoPost státusz beállítás történik
        /// </summary>
        /// <param name="cartId"></param>
        public void Post(string cartId, PaymentTerms paymentTerms, DeliveryTerms deliveryTerms, CompanyGroup.Domain.WebshopModule.Shipping shipping)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(cartId));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", MongoDB.Bson.BsonInt32.Create(CartStatus.WaitingForAutoPost))
                    .Set("PaymentTerms", MongoDB.Bson.BsonInt32.Create(paymentTerms))
                    .Set("DeliveryTerms", MongoDB.Bson.BsonInt32.Create(deliveryTerms))
                    .Set("Shipping.AddrRecId", MongoDB.Bson.BsonInt64.Create(shipping.AddrRecId))
                    .Set("Shipping.City", MongoDB.Bson.BsonString.Create(shipping.City))
                    .Set("Shipping.DateRequested", MongoDB.Bson.BsonDateTime.Create(shipping.DateRequested))
                    .Set("Shipping.InvoiceAttached", MongoDB.Bson.BsonBoolean.Create(shipping.InvoiceAttached))
                    .Set("Shipping.Street", MongoDB.Bson.BsonString.Create(shipping.Street))
                    .Set("Shipping.ZipCode", MongoDB.Bson.BsonString.Create(shipping.ZipCode));

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// finanszírozási ajánlat feladása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="financeOffer"></param>
        public void PostFinanceOffer(string cartId, CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", this.ConvertStringToBsonObjectId(cartId));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", MongoDB.Bson.BsonInt32.Create(CartStatus.FinanceOfferPosted))
                    .Set("FinanceOffer.Address", MongoDB.Bson.BsonString.Create(financeOffer.Address))
                    .Set("FinanceOffer.NumOfMonth", MongoDB.Bson.BsonInt32.Create(financeOffer.NumOfMonth))
                    .Set("FinanceOffer.PersonName", MongoDB.Bson.BsonString.Create(financeOffer.PersonName))
                    .Set("FinanceOffer.Phone", MongoDB.Bson.BsonString.Create(financeOffer.Phone))
                    .Set("FinanceOffer.StatNumber", MongoDB.Bson.BsonString.Create(financeOffer.StatNumber));

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// kosár letárolása későbbi felhasználásra
        /// "Status" mező értékének "Stored"-re állítása, név értékének beállítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="name"></param>
        public void Store(string cartId, string name)
        { 
            this.Store(this.ConvertStringToBsonObjectId(cartId), name);
        }

        /// <summary>
        /// kosár letárolása későbbi felhasználásra
        /// "Status" mező értékének "Stored"-re állítása, név értékének beállítása
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="name"></param>
        public void Store(MongoDB.Bson.ObjectId cartId, string name)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", cartId);

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Status", CartStatus.Stored).Set("Name", name);

                collection.Update(query, update);

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        public void SetActive(string cartId, bool active)
        {
            this.SetActive(this.ConvertStringToBsonObjectId(cartId), active);
        }

        /// <summary>
        /// kosár aktív / inaktív beállítása
        /// beállítja az "id" alapján az "Active" mező értékét "false"-ra, vagy "true"-ra.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        public void SetActive(MongoDB.Bson.ObjectId cartId, bool active)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", cartId);

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Active", active);

                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// létezik-e a productId-vel rendelkező termék a cartId azonosítójú kosárban?
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool ExistsProductInCart(string cartId, string productId)
        {
            return ExistsProductInCart(this.ConvertStringToBsonObjectId(cartId), productId);
        }

        /// <summary>
        /// létezik-e a productId-vel rendelkező termék a cartId azonosítójú kosárban?
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool ExistsProductInCart(MongoDB.Bson.ObjectId cartId, string productId) 
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", cartId),
                                                                      MongoDB.Driver.Builders.Query.EQ("Items.ProductId", productId));

                return collection.Count(query) > 0;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// új visitorId beállítása a régi helyére
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="visitorId"></param>
        public void AssociateCart(string cartId, string visitorId)
        {
            AssociateCart(this.ConvertStringToBsonObjectId(cartId), visitorId);
        }

        /// <summary>
        /// új visitorId beállítása a régi helyére
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="visitorId"></param>
        public void AssociateCart(MongoDB.Bson.ObjectId cartId, string visitorId)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", cartId),
                                                                      MongoDB.Driver.Builders.Query.EQ("Status", MongoDB.Bson.BsonInt32.Create(CartStatus.Stored)));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("VisitorId", visitorId);

                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }        
        }

        #region "kosár tételhez kapcsolódó műveletek"

        /// <summary>
        /// kosár elem mennyiség frissítése
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        public void UpdateLineQuantity(string cartId, string productId, int quantity)
        {
            this.UpdateLineQuantity(this.ConvertStringToBsonObjectId(cartId), productId, quantity);
        }

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
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        public void UpdateLineQuantity(MongoDB.Bson.ObjectId cartId, string productId, int quantity)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", cartId),
                                                                      MongoDB.Driver.Builders.Query.EQ("Items.ProductId", productId));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Items.$.Quantity", quantity);

                //MongoCursor<CompanyGroup.Domain.WebshopModule.ShoppingCart> cart = 
                collection.Update(query, update);

                //collection.Update(MongoDB.Driver.Builders.Query.EQ("Id", product.Id), MongoDB.Driver.Builders.Update.Pull("Pictures", MongoDB.Driver.Builders.Query.EQ("FileName", MongoDB.Bson.BsonValue.Create(picture.FileName))));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
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
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("_id", item.Id);

                collection.Update(query, MongoDB.Driver.Builders.Update.PushWrapped("Items", item));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        public void RemoveLine(string cartId, string productId)
        {
            this.RemoveLine(this.ConvertStringToBsonObjectId(cartId), productId);
        }

        /// <summary>
        ///  _posts.Collection.Update(Query.EQ("_id", postId), Update.Pull("Comments", Query.EQ("_id", commentId)).Inc("TotalComments", -1));
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        public void RemoveLine(MongoDB.Bson.ObjectId cartId, string productId)
        {
            try
            {
                this.ReConnect();

                MongoDB.Driver.MongoCollection<CompanyGroup.Domain.WebshopModule.ShoppingCart> collection = this.GetCollection(ShoppingCartRepository.CollectionName);

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", cartId),
                                                      MongoDB.Driver.Builders.Query.EQ("Items.ProductId", productId));

                IMongoUpdate update = MongoDB.Driver.Builders.Update.Set("Items.$.Status", CartItemStatus.Deleted);

                collection.Update(query, update);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                this.Disconnect();
            }        
        }

        #endregion
    }
}
