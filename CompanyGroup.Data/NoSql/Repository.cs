using System;
using System.Linq;
using System.Collections.Generic;

namespace CompanyGroup.Data.NoSql
{
    /// <summary>
    /// NoSql repository 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Repository<T> : RepositoryBase, CompanyGroup.Domain.Core.IRepository<T> where T : class
    {

        private ISettings settings;

        private MongoDB.Driver.MongoServer server = null;

        private MongoDB.Driver.MongoDatabase database = null;

        private bool connected = false;

        /// <summary>
        /// konstruktor, kapcsolat felépítése a szerverrel, adatbázis kapcsolat inicializálása
        /// </summary>
        /// <param name="unitOfWork">mongodb settings</param>
        public Repository(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.Connect(settings);
        }

        private void Connect(ISettings settings)
        {
            this.settings = settings;

            //  MongoDB.Driver.MongoConnectionStringBuilder connectionStringBuilder = new MongoDB.Driver.MongoConnectionStringBuilder(settings.ConnectionString);

            this.server = MongoDB.Driver.MongoServer.Create(settings.ConnectionString);

            this.database = server.GetDatabase(settings.Database);

            this.connected = true;
        }

        /// <summary>
        /// újra konnektálás
        /// </summary>
        /// <param name="settings"></param>
        public void ReConnect(ISettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.Connect(settings);            
        }

        /// <summary>
        /// újra konnektálás
        /// </summary>
        public void ReConnect()
        {
            if (this.settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if (!this.connected)
            {
                this.Connect(this.settings);
            }
        }

        /// <summary>
        /// mongo kollekció kiolvasása
        /// </summary>
        /// <returns></returns>
        public MongoDB.Driver.MongoCollection<T> GetCollection()
        {
            return this.GetCollection(settings.Collection);
        }

        /// <summary>
        /// mongo kollekció kiolvasása kollekció név alapján 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public MongoDB.Driver.MongoCollection<T> GetCollection(string collection)
        {
            return this.database.GetCollection<T>(collection);
        }

        /// <summary>
        /// kapcsolat bontása a szerverrel
        /// </summary>
        public void Disconnect()
        {
            try
            {
                this.connected = false;

                if (this.server == null) { return; }

                this.database = null;

                this.server.Disconnect();
            }
            catch { }
        }

        /// <summary>
        /// kiüríti az elemeket a megadott kollekcióból
        /// </summary>
        /// <param name="dataAreaId"></param>
        public virtual void RemoveItemsFromCollection(string dataAreaId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "DataareaId may not be null nor empty");

                MongoDB.Driver.IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId);

                MongoDB.Driver.SafeModeResult res = this.GetCollection().Remove(query);
            }
            catch
            {
            }
            finally
            {
                Disconnect();
            }
        }

        public virtual void RemoveAllItemsFromCollection()
        {
            this.GetCollection().RemoveAll();
        }

        /// <summary>
        /// törli a kollekciót a megadott settings.Collection alapján
        /// </summary>
        /// <returns></returns>
        public virtual void DropCollection()
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(settings.Collection), "CollectionName may not be null nor empty");

                this.database.DropCollection(settings.Collection);
            }
            catch { }
        }

        /// <summary>
        /// törli a kollekcióhoz tartozó összes indexet 
        /// </summary>
        /// <returns></returns>
        public virtual void DeleteIndexes()
        {
            try
            {
                MongoDB.Driver.MongoCollection<T> collection = this.GetCollection();

                collection.DropAllIndexes();

                return;
            }
            catch
            {
                return;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// DB.GetCollection[User](Dbname)
        ///.EnsureIndex(new IndexKeysBuilder()
        ///.Ascending("EmailAddress"), IndexOptions.SetUnique(true));
        /// </example>
        public virtual void CreateIndexes()
        {
            try
            {
                MongoDB.Driver.MongoCollection<T> collection = this.GetCollection();

                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId", "Structure.Manufacturer.ManufacturerId", "ProductName"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_ManufacturerId_ProductName"));
                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId", "Structure.Category1.CategoryId", "ProductName"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_Category1Id_ProductName"));
                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId", "Structure.Manufacturer.ManufacturerId", "ProductId"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_ManufacturerId_ProductId"));
                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId", "ProductName"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_ProductName"));
                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId", "ProductId"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_ProductId"));
                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId", "PartNumber"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_PartNumber"));

                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId").Descending("AverageInventory"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_AverageInventory"));
                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId", "Prices.Price5", "ProductName"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_Price_ProductName_Asc"));

                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("DataAreaId").Descending("Prices.Price5").Ascending("ProductName"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_DataAreaId_Price_ProductName_Desc"));

                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("ProductName"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_ProductName_Asc"));
                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Descending("ProductName"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_ProductName_Desc"));

                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Ascending("ProductId"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_ProductId_Asc"));
                collection.EnsureIndex(new MongoDB.Driver.Builders.IndexKeysBuilder().Descending("ProductId"), MongoDB.Driver.Builders.IndexOptions.SetName("Idx_ProductId_Desc"));

                return;
            }
            catch
            {
                return;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// convert string identity to nosql objectId
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        protected MongoDB.Bson.ObjectId ConvertStringToBsonObjectId(string from)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(from), "Object identifier may not be null nor empty");

                MongoDB.Bson.ObjectId to = MongoDB.Bson.ObjectId.Empty;

                bool b = MongoDB.Bson.ObjectId.TryParse(from, out to);

                return (b) ? to : MongoDB.Bson.ObjectId.Empty;
            }
            catch { return MongoDB.Bson.ObjectId.Empty; }
        }

        /// <summary>
        /// lekérdezés paraméterek felépítése
        /// </summary>
        /// <param name="manufacturerIdList"></param>
        /// <param name="category1IdList"></param>
        /// <param name="category2IdList"></param>
        /// <param name="category3IdList"></param>
        /// <param name="actionFilter"></param>
        /// <param name="bargainFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        protected MongoDB.Driver.IMongoQuery ConstructQueryDocument(List<string> manufacturerIdList,
                                                                               List<string> category1IdList,
                                                                               List<string> category2IdList,
                                                                               List<string> category3IdList,
                                                                               bool actionFilter,
                                                                               bool bargainFilter,
                                                                               bool isInNewsletterFilter,
                                                                               bool newFilter,
                                                                               bool stockFilter,
                                                                               string textFilter,
                                                                               string priceFilter,
                                                                               int priceFilterRelation,
                                                                               string nameOrPartNumberFilter, 
                                                                               string dataAreaId)
        {
            bool isCommonCompany = (dataAreaId == "");

            MongoDB.Driver.IMongoQuery queryComplete = isCommonCompany ? MongoDB.Driver.Builders.Query.NE("DataAreaId", CompanyGroup.Domain.Core.Constants.DataAreaIdSerbia) : MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId);

            queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.LT("ItemState", 2));

            if (manufacturerIdList.Count > 0)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.In("Structure.Manufacturer.ManufacturerId", MongoDB.Bson.BsonArray.Create(manufacturerIdList)));
            }
            if (category1IdList.Count > 0)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.In("Structure.Category1.CategoryId", MongoDB.Bson.BsonArray.Create(category1IdList)));
            }
            if (category2IdList.Count > 0)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.In("Structure.Category2.CategoryId", MongoDB.Bson.BsonArray.Create(category2IdList)));
            }
            if (category3IdList.Count > 0)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.In("Structure.Category3.CategoryId", MongoDB.Bson.BsonArray.Create(category3IdList)));
            }
            if (actionFilter)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Discount", actionFilter));
            }
            if (bargainFilter)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.Where("this.SecondHandList.length > 0"));
            }
            if (isInNewsletterFilter)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("IsInNewsletter", isInNewsletterFilter));
            }
            if (newFilter)
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("New", newFilter));
            }
            if (stockFilter)
            {
                if (isCommonCompany)
                {
                    queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                        MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.GT("Stock.Inner", 0),
                                                         MongoDB.Driver.Builders.Query.GT("Stock.Outer", 0))
                        );
                }
                else
                {
                    queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                        MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.GT("Stock.Serbian", 0),
                                                         MongoDB.Driver.Builders.Query.GT("Stock.Inner", 0),
                                                         MongoDB.Driver.Builders.Query.GT("Stock.Outer", 0))
                        );
                }
            }

            if (!String.IsNullOrEmpty(textFilter))
            {
                //MongoDB.Bson.BsonRegularExpression regex = new MongoDB.Bson.BsonRegularExpression(String.Format(".*{0}.*", textFilter), "i");

                MongoDB.Bson.BsonRegularExpression regex = MongoDB.Bson.BsonRegularExpression.Create(new System.Text.RegularExpressions.Regex(textFilter, System.Text.RegularExpressions.RegexOptions.IgnoreCase));

                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                    MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.Matches("ProductName", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("ProductNameEnglish", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("Description", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("DescriptionEnglish", regex))
                        );
            }
 
            if (!String.IsNullOrEmpty(nameOrPartNumberFilter))
            {
                MongoDB.Bson.BsonRegularExpression regex = new MongoDB.Bson.BsonRegularExpression(String.Format(".*{0}.*", nameOrPartNumberFilter), "i");

                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                    MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.Matches("ProductName", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("ProductNameEnglish", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("PartNumber", regex),
                                                     MongoDB.Driver.Builders.Query.Matches("ProductId", regex))
                    );
            }

            if (priceFilterRelation.Equals(1))
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.GTE("Prices.Price2", MongoDB.Bson.BsonInt32.Create(priceFilter)));
            }
            else if (priceFilterRelation.Equals(2))
            {
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.LTE("Prices.Price2", MongoDB.Bson.BsonInt32.Create(priceFilter)));
            }

            return queryComplete == null ? MongoDB.Driver.Builders.Query.Null : queryComplete;
        }

        /// <summary>
        /// Bson documentum konvertálása string típusra
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        protected string ConvertBsonToString(MongoDB.Bson.BsonValue from)
        {
            try
            {
                return from.AsString;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected enum SortDirection { Ascending, Decending }

        protected List<T> Sort(
            IEnumerable<T> list,
            System.Linq.Expressions.ParameterExpression prmExpression,
            string sortExpression,
            SortDirection sortDirection)
        {
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(System.Linq.Expressions.Expression.Convert(System.Linq.Expressions.Expression.Property(prmExpression, sortExpression), typeof(object)), prmExpression);
            return sortDirection == SortDirection.Ascending ?
                list.AsQueryable<T>().OrderBy<T, object>(lambda).ToList() :
                list.AsQueryable<T>().OrderByDescending<T, object>(lambda).ToList();
        }

        //public IOrderedEnumerable<TList> Sort<TKey, TList>(ref List<TList> list, Func<TList, TKey> sorter, SortDirection direction)
        //{
        //    if (direction == SortDirection.Ascending)
        //        return list.OrderBy(sorter);
        //    else
        //        return list.OrderByDescending(sorter);
        //}

        //public static void SortAsc<TSource, TValue>(
        //    this List<TSource> source,
        //    Func<TSource, TValue> selector)
        //{
        //    var comparer = Comparer<TValue>.Default;
        //    source.Sort((x, y) => comparer.Compare(selector(x), selector(y)));
        //}

        //public static void SortDesc<TSource, TValue>(
        //  this List<TSource> source,
        //  Func<TSource, TValue> selector)
        //{
        //    var comparer = Comparer<TValue>.Default;
        //    source.Sort((x, y) => comparer.Compare(selector(y), selector(x)));
        //}


    }
}
