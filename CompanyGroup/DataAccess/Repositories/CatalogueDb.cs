
namespace Shared.Web.DataAccess.NoSql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;
    using MongoDB.Driver;

    /// <summary>
    /// cikk katalógus kollekció
    /// </summary>
    public class CatalogueDb : BaseDb<Shared.Web.NoSql.Entities.CatalogueItem>, ICatalogueDb   
    {
        /// <summary>
        /// cikk katalógus kollekció konstruktor
        /// </summary>
        public CatalogueDb() : base("Catalogue") { }

        #region "index karbantartas"

        //private static readonly string Index_Catalogue_Action_Asc = "Catalogue_Action_Asc";
        //private static readonly string Index_Catalogue_BargainCounter_Asc = "Catalogue_BargainCounter_Asc";
        //private static readonly string Index_Catalogue_FocusWeek_Asc = "Catalogue_FocusWeek_Asc";
        //private static readonly string Index_Catalogue_New_Asc = "Catalogue_New_Asc";
        //private static readonly string Index_Catalogue_Top10_Asc = "Catalogue_Top10_Asc";

        //private static readonly string Index_Catalogue_ManufacturerId_Asc = "Catalogue_ManufacturerId_Asc";
        //private static readonly string Index_Catalogue_Category1Id_Asc = "Catalogue_Category1Id_Asc";
        //private static readonly string Index_Catalogue_Category2Id_Asc = "Catalogue_Category2Id_Asc";
        //private static readonly string Index_Catalogue_Category3Id_Asc = "Catalogue_Category3Id_Asc";
        ////private static readonly string Index_Catalogue_ManufacturerId_Asc = "Catalogue_ManufacturerId_Category1Id_Asc";

        //private static readonly string Index_Catalogue_ItemName_Asc = "Catalogue_ItemName_Asc";
        //private static readonly string Index_Catalogue_ItemName_Desc = "Catalogue_ItemName_Desc";

        //private static readonly string Index_Catalogue_PartNumber_Asc = "Catalogue_PartNumber_Asc";
        
        //private static readonly string Index_Catalogue_ProductId_Asc = "Catalogue_ProductId_Asc";
        //private static readonly string Index_Catalogue_ProductId_Desc = "Catalogue_ProductId_Desc";

        //private static readonly string Index_Catalogue_Price1_Asc = "Catalogue_Price1_Asc";
        //private static readonly string Index_Catalogue_Price1_Desc = "Catalogue_Price1_Desc";

        //private static readonly string Index_Catalogue_Price2_Asc = "Catalogue_Price2_Asc";
        //private static readonly string Index_Catalogue_Price2_Desc = "Catalogue_Price2_Desc";

        //private static readonly string Index_Catalogue_Price3_Asc = "Catalogue_Price3_Asc";
        //private static readonly string Index_Catalogue_Price3_Desc = "Catalogue_Price3_Desc";

        //private static readonly string Index_Catalogue_Price4_Asc = "Catalogue_Price4_Asc";
        //private static readonly string Index_Catalogue_Price4_Desc = "Catalogue_Price4_Desc";

        //private static readonly string Index_Catalogue_Price5_Asc = "Catalogue_Price5_Asc";
        //private static readonly string Index_Catalogue_Price5_Desc = "Catalogue_Price5_Desc";

        //private static readonly string Index_Catalogue_Price6_Asc = "Catalogue_Price6_Asc";
        //private static readonly string Index_Catalogue_Price6_Desc = "Catalogue_Price6_Desc";

        /// <summary>
        /// cikk kollekció indexek létrehozása
        /// </summary>
        /// <returns></returns>
        public bool CreateIndexes()
        {
            try
            {
                ConnectToDatabase();

                MongoCollection<Shared.Web.NoSql.Entities.CatalogueItem> collection = this.GetCollection();

                //MongoDB.Driver.Builders.IndexKeys.Ascending;
                //MongoDB.Driver.Builders.IndexOptions. ;

                //var keybuilder = new MongoDB.Driver.Builders.IndexKeysBuilder();
                //var optionbuilder = new MongoDB.Driver.Builders.IndexOptionsBuilder();
                //optionbuilder.SetName("Index_Catalogue_Price1_Desc");

                //indexkeys, indexoptionbuilder
                //collection.CreateIndex(keybuilder, optionbuilder); 

                //szurofeltetel indexek
                collection.CreateIndex(new string[] { "Action" });

                collection.CreateIndex(new string[] { "BargainCounter" });

                collection.CreateIndex(new string[] { "FocusWeek" });

                collection.CreateIndex(new string[] { "New" });

                collection.CreateIndex(new string[] { "Top10" });

                //struktura index 
                collection.CreateIndex(new string[] { "ManufacturerName", "Category1Name", "Category2Name", "Category3Name" });

                //cikk attributum indexek
                collection.CreateIndex(new string[] { "ItemName" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "ItemName" }));

                //cikkszám indexek
                collection.CreateIndex(new string[] { "PartNumber" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "PartNumber" }));

                //termékazonosító indexek
                collection.CreateIndex(new string[] { "ProductId" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "ProductId" }));

                //ár indexek 
                collection.CreateIndex(new string[] { "Price1" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "Price1" }));
                collection.CreateIndex(new string[] { "Price2" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "Price2" }));
                collection.CreateIndex(new string[] { "Price3" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "Price3" }));
                collection.CreateIndex(new string[] { "Price4" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "Price4" }));
                collection.CreateIndex(new string[] { "Price5" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "Price5" }));
                collection.CreateIndex(new string[] { "Price6" });
                collection.CreateIndex(MongoDB.Driver.Builders.IndexKeys.Descending(new string[] { "Price6" }));

                return true;
            }
            catch 
            {
                return false;
            }
            finally 
            {
                DisconnectFromDatabase();
            }
        }

        #endregion

        #region "nosql adatok lekérdezése"

        #region "termékkatalógus"

        /// <summary>
        /// termékkatalógus lekérdezés 
        /// 1. lekérdezés paramétereinek összeállítása BsonDocument query dokumentumba,
        /// 2. NoSql dokumentum kollekció lekérdezése, 
        /// 3. Lekérdezés által visszaadott eredménykollekció feldolgozása, 
        /// 4. NoSql cikk típusú lista létrehozása és feltöltése a leválogatásból származó eredménykollekcióval  
        /// </summary>
        /// <remarks>sorrendezés és lapozás, szűrés</remarks>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="actionFilter"></param>
        /// <param name="bargainFilter"></param>
        /// <param name="focusweekFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="top10Filter"></param>
        /// <param name="textFilter"></param>
        /// <param name="sequence"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="itemsOnPage"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Shared.Web.NoSql.Entities.CatalogueItem> GetProductCatalogue( string dataAreaId, 
                                                                                  string manufacturerId, 
                                                                                  string category1Id, 
                                                                                  string category2Id, 
                                                                                  string category3Id,
                                                                                  bool actionFilter, 
                                                                                  bool bargainFilter, 
                                                                                  bool focusweekFilter, 
                                                                                  bool newFilter, 
                                                                                  bool stockFilter, 
                                                                                  bool top10Filter,
                                                                                  string textFilter, 
                                                                                  int sequence, 
                                                                                  int currentPageIndex, 
                                                                                  int itemsOnPage, 
                                                                                  ref int count )
        {
            try
            {
                ConnectToDatabase();

                MongoDB.Driver.Builders.QueryComplete query = ConstructQueryDocument( manufacturerId, category1Id, category2Id, category3Id,
                                                                                      actionFilter, bargainFilter, focusweekFilter, newFilter, stockFilter, top10Filter, 
                                                                                      textFilter, dataAreaId );

                MongoCollection<BsonDocument> catalogue = this.Database.GetCollection("Catalogue");

                count = catalogue.Find(query).Count();

                //IMongoSortBy

                MongoCursor<BsonDocument> filteredList = catalogue.Find(query)
                                                                  .SetSortOrder( GetSortOrderFieldName(sequence) )
                                                                  .SetSkip(currentPageIndex * itemsOnPage)
                                                                  .SetLimit(itemsOnPage);

                List<Shared.Web.NoSql.Entities.CatalogueItem> resultList = new List<Shared.Web.NoSql.Entities.CatalogueItem>();

                foreach (BsonDocument item in filteredList) 
                {
                    resultList.Add(
                       new Shared.Web.NoSql.Entities.CatalogueItem()
                       {
                           Action = item["Action"].IsBoolean ? item["Action"].AsBoolean : false,
                           AgentItem = ConvertBsonDocumentToAgentItem(item["AgentItem"].AsBsonDocument),
                           BargainCounter = item["BargainCounter"].IsBoolean ? item["BargainCounter"].AsBoolean : false,
                           BscInnerStock = item["BscInnerStock"].IsInt32 ? item["BscInnerStock"].AsInt32 : 0,
                           BscOuterStock = item["BscOuterStock"].IsInt32 ? item["BscOuterStock"].AsInt32 : 0,
                           Category1Id = item["Category1Id"].IsString ? item["Category1Id"].AsString : String.Empty,
                           Category1Name = item["Category1Name"].IsString ? item["Category1Name"].AsString : String.Empty,
                           Category1NameEnglish = item["Category1NameEnglish"].IsString ? item["Category1NameEnglish"].AsString : String.Empty,
                           Category2Id = item["Category2Id"].IsString ? item["Category2Id"].AsString : String.Empty,
                           Category2Name = item["Category2Name"].IsString ? item["Category2Name"].AsString : String.Empty,
                           Category2NameEnglish = item["Category2NameEnglish"].IsString ? item["Category2NameEnglish"].AsString : String.Empty,
                           Category3Id = item["Category3Id"].IsString ? item["Category3Id"].AsString : String.Empty,
                           Category3Name = item["Category3Name"].IsString ? item["Category3Name"].AsString : String.Empty,
                           Category3NameEnglish = item["Category3NameEnglish"].IsString ? item["Category3NameEnglish"].AsString : String.Empty,
                           Currency = item["Currency"].IsString ? item["Currency"].AsString : String.Empty,
                           DataAreaId = item["DataAreaId"].IsString ? item["DataAreaId"].AsString : String.Empty,
                           Description = item["Description"].IsString ? item["Description"].AsString : String.Empty,
                           DescriptionEnglish = item["DescriptionEnglish"].IsString ? item["DescriptionEnglish"].AsString : String.Empty,
                           FocusWeek = item["FocusWeek"].IsBoolean ? item["FocusWeek"].AsBoolean : false,
                           Garanty = item["Garanty"].IsString ? item["Garanty"].AsString : String.Empty,
                           GarantyMode = item["GarantyMode"].IsString ? item["GarantyMode"].AsString : String.Empty,
                           HrpInnerStock = item["HrpInnerStock"].IsInt32 ? item["HrpInnerStock"].AsInt32 : 0,
                           HrpOuterStock = item["HrpOuterStock"].IsInt32 ? item["HrpOuterStock"].AsInt32 : 0,
                           ItemName = item["ItemName"].IsString ? item["ItemName"].AsString : String.Empty,
                           ItemNameEnglish = item["ItemNameEnglish"].IsString ? item["ItemNameEnglish"].AsString : String.Empty,
                           ItemState = item["ItemState"].IsInt32 ? item["ItemState"].AsInt32 : 0,
                           ManufacturerId = item["ManufacturerId"].IsString ? item["ManufacturerId"].AsString : String.Empty,
                           ManufacturerName = item["ManufacturerName"].IsString ? item["ManufacturerName"].AsString : String.Empty,
                           ManufacturerNameEnglish = item["ManufacturerNameEnglish"].IsString ? item["ManufacturerNameEnglish"].AsString : String.Empty,
                           New = item["New"].IsBoolean ? item["New"].AsBoolean : false,
                           SerbianStock = item["SerbianStock"].IsInt32 ? item["SerbianStock"].AsInt32 : 0,
                           PartNumber = item["PartNumber"].IsString ? item["PartNumber"].AsString : String.Empty,
                           ProductId = item["ProductId"].IsString ? item["ProductId"].AsString : String.Empty,
                           Top10 = item["Top10"].IsBoolean ? item["Top10"].AsBoolean : false,
                           Price1 = item["Price1"].IsDouble ? item["Price1"].AsDouble : 0,
                           Price2 = item["Price2"].IsDouble ? item["Price2"].AsDouble : 0,
                           Price3 = item["Price3"].IsDouble ? item["Price3"].AsDouble : 0,
                           Price4 = item["Price4"].IsDouble ? item["Price4"].AsDouble : 0,
                           Price5 = item["Price5"].IsDouble ? item["Price5"].AsDouble : 0,
                           Price6 = item["Price6"].IsDouble ? item["Price6"].AsDouble : 0, 
                           Pictures = ConvertBsonArrayToPictureList( item["Pictures"].AsBsonArray )
                       }
                    );
                }
                return resultList;
            }
            catch
            {
                return new List<Shared.Web.NoSql.Entities.CatalogueItem>();
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        /// <summary>
        /// Query dokumentum összeállítása 
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="actionFilter"></param>
        /// <param name="bargainFilter"></param>
        /// <param name="focusweekFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="top10Filter"></param>
        /// <param name="textFilter"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        private MongoDB.Driver.Builders.QueryComplete ConstructQueryDocument(string manufacturerId, 
                                                                             string category1Id, 
                                                                             string category2Id, 
                                                                             string category3Id,
                                                                             bool actionFilter, 
                                                                             bool bargainFilter, 
                                                                             bool focusweekFilter, 
                                                                             bool newFilter, 
                                                                             bool stockFilter, 
                                                                             bool top10Filter, 
                                                                             string textFilter, 
                                                                             string dataAreaId)
        {
            //QueryDocument query = new QueryDocument();

            /*
            MongoDB.Driver.Builders.QueryComplete queryComplete = MongoDB.Driver.Builders.Query.And("Date", minDate).LTE(maxDate); 

            MongoDB.Driver.Builders.Query.Or(
                    MongoDB.Driver.Builders.Query.EQ("name", "Flav"),
                    MongoDB.Driver.Builders.Query.EQ("name", "Flavien"),
                    MongoDB.Driver.Builders.Query.EQ("value", "Flavien")); 
            */

            bool isCommonCompany = !(dataAreaId.ToLower().Equals(DataAreaIdSerbia));

            MongoDB.Driver.Builders.QueryComplete queryComplete = isCommonCompany ? MongoDB.Driver.Builders.Query.NE("DataAreaId", DataAreaIdSerbia) : MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId);

            queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.LT("ItemState", 2));

            //List<MongoDB.Driver.Builders.QueryComplete> qcList = new List<MongoDB.Driver.Builders.QueryComplete>();

            if (!String.IsNullOrEmpty(manufacturerId))
            {
                //query.Add("ManufacturerId", manufacturerId);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("ManufacturerId", manufacturerId));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("ManufacturerId", manufacturerId));

            }
            if (!String.IsNullOrEmpty(category1Id))
            {
                //query.Add("Category1Id", category1Id);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Category1Id", category1Id));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("Category1Id", category1Id));
            }
            if (!String.IsNullOrEmpty(category2Id))
            {
                //query.Add("Category2Id", category2Id);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Category2Id", category2Id));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("Category2Id", category2Id));
            }
            if (!String.IsNullOrEmpty(category3Id))
            {
                //query.Add("Category3Id", category3Id);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Category3Id", category3Id));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("Category3Id", category3Id));
            }
            if (actionFilter)
            {
                //query.Add("Action", actionFilter);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Action", actionFilter));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("Action", actionFilter));
            }
            if (bargainFilter)
            {
                //query.Add("BargainCounter", bargainFilter);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("BargainCounter", bargainFilter));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("BargainCounter", bargainFilter));
            }
            if (focusweekFilter)
            {
                //query.Add("FocusWeek", focusweekFilter);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("FocusWeek", focusweekFilter));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("FocusWeek", focusweekFilter));
            }
            if (newFilter)
            {
                //query.Add("New", newFilter);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("New", newFilter));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("New", newFilter));
            }
            if (stockFilter)
            {

                if (isCommonCompany)
                {
                    //query.Add("InStock", stockFilter);
                    queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                        MongoDB.Driver.Builders.Query.Or( MongoDB.Driver.Builders.Query.GT("HrpInnerStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("BscInnerStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("HrpOuterStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("BscOuterStock", 0))
                        );
                }
                else
                {
                    queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                        MongoDB.Driver.Builders.Query.Or( MongoDB.Driver.Builders.Query.GT("SerbianStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("HrpInnerStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("BscInnerStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("HrpOuterStock", 0),
                                                          MongoDB.Driver.Builders.Query.GT("BscOuterStock", 0))
                        );                    
                }
            }
            if (top10Filter)
            {
                //query.Add("Top10", top10Filter);
                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("Top10", top10Filter));

                //qcList.Add(MongoDB.Driver.Builders.Query.EQ("Top10", top10Filter));
            }
            if (!String.IsNullOrEmpty(textFilter))
            {
                //query.Add("ItemName", new BsonRegularExpression(string.Format("^{0}", textFilter), "i"));
                //query.Add("Description", new BsonRegularExpression(string.Format("^{0}", textFilter), "i"));
                //BsonRegularExpression regex1 = new BsonRegularExpression(string.Format("^{0}", textFilter), "i");
                BsonRegularExpression regex = new BsonRegularExpression(String.Format(".*{0}.*", textFilter), "i");

                //queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.Matches("ItemName", regex));
                
                    queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete,

                        MongoDB.Driver.Builders.Query.Or(MongoDB.Driver.Builders.Query.Matches("ItemName", regex),
                                                         MongoDB.Driver.Builders.Query.Matches("ItemNameEnglish", regex),
                                                         MongoDB.Driver.Builders.Query.Matches("Description", regex),
                                                         MongoDB.Driver.Builders.Query.Matches("DescriptionEnglish", regex))
                        );


                //spec.Add("ItemName", new MongoRegex(".*" + searchKey + ".*", "i"));


                //qcList.Add(MongoDB.Driver.Builders.Query.Matches("ItemName", regex2));
            }

            //qcList.ForEach(delegate(MongoDB.Driver.Builders.QueryComplete item)
            //{
            //    query.Add( item.ToBsonDocument());
            //});

            //string s = query.ToString();

            return queryComplete == null ? new MongoDB.Driver.Builders.QueryComplete(new BsonDocument()) : queryComplete;
        }

        /// <summary>
        /// Bson array dokumentum konvertálása NoSql kép típusba
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private List<Shared.Web.NoSql.Entities.Picture> ConvertBsonArrayToPictureList( BsonArray array )
        {
            List<Shared.Web.NoSql.Entities.Picture> list = new List<Shared.Web.NoSql.Entities.Picture>();
            for (int i = 0; i < array.Count; i++)
            {
                BsonDocument doc = array[i].AsBsonDocument;
                list.Add(new Shared.Web.NoSql.Entities.Picture()
                              {
                                  Primary = doc["Primary"].IsBoolean ? doc["Primary"].AsBoolean : false,
                                  RecId = doc["RecId"].IsInt64 ? doc["RecId"].AsInt64 : 0, 
                                  FileName = doc["FileName"].IsString ? doc["FileName"].AsString : String.Empty
                              } );
            }
            return list;
        }

        /// <summary>
        /// elsődleges kép rekordazonosítójának kiolvasása 
        /// </summary>
        /// <param name="array">Bson dokumentum tömb</param>
        /// <returns>picture rekord</returns>
        private Shared.Web.NoSql.Entities.Picture GetPrimaryPicture(BsonArray array)
        {
            List<Shared.Web.NoSql.Entities.Picture> list = ConvertBsonArrayToPictureList(array);

            if (list.Count == 0) { return new Web.NoSql.Entities.Picture(); }

            Shared.Web.NoSql.Entities.Picture picture = list.Where(item => item.Primary == true).FirstOrDefault();

            if (picture == null) { picture = list.FirstOrDefault(); }

            return (picture != null) ? picture : new Shared.Web.NoSql.Entities.Picture();
        }

        /// <summary>
        /// Bson dokumentum NoSql agent (képviselő) típusra történő konvertálása
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private Shared.Web.NoSql.Entities.Agent ConvertBsonDocumentToAgentItem(BsonDocument document)
        {
            try
            {
                return new Shared.Web.NoSql.Entities.Agent()
                    {
                        Email = document["Email"].AsString,
                        EmpId = document["EmpId"].AsString,
                        Mobile = document["Mobile"].AsString,
                        Name = document["Name"].AsString,
                        Phone = document["Phone"].AsString
                    };
            }
            catch { return new Shared.Web.NoSql.Entities.Agent(); }
        }


        /// <summary>
        /// katalógus elemeket tartalmazó lista sorrendezése
        /// 0: gyarto es jellegek növekvő,
        /// 1: gyarto es jellegek csökkenő, 
        /// 2: azonosito növekvő,
        /// 3: azonosito csökkenő,
        /// 4 nev növekvő,
        /// 5: nev csökkenő,
        /// 6: ar növekvő,
        /// 7 ar csökkenő, 
        /// 8: jelleg1 + jelleg2 + jelleg3 + gyarto növekvő, 
        /// 9: jelleg1 + jelleg2 + jelleg3 + gyarto csökkenő
        /// 10: jelleg2 + jelleg3 + jelleg1 + gyarto növekvő
        /// 11: jelleg2 + jelleg3 + jelleg1 + gyarto csökkenő
        /// 12: jelleg3 + jelleg2 + jelleg1 + gyarto növekvő
        /// 13: jelleg3 + jelleg2 + jelleg1 + gyarto csökkenő
        /// 14 : belső készlet növekvő,
        /// 15 : belső készlet csökkenő,
        /// 16 : külső készlet növekvő,
        /// 17 : külső készlet csökkenő,
        /// 18 : készlet növekvő,
        /// 19 : készlet csökkenő,
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private IMongoSortBy GetSortOrderFieldName(int sequence)
        {
            //List<string> fields = new List<string>();

            MongoDB.Driver.Builders.SortByBuilder builder;

            switch (sequence)
            {
                case 0:
                    {
                        //0: gyarto es jellegek novekvo,
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("ManufacturerName").Ascending("Category1Name").Ascending("Category2Name").Ascending("Category3Name");
                        //fields.Add( "ManufacturerName" );
                        break;
                    }
                case 1:
                    {
                        //1: gyarto es jellegek csokkeno
                        builder = MongoDB.Driver.Builders.SortBy.Descending("ManufacturerName").Descending("Category1Name").Descending("Category2Name").Descending("Category3Name");
                        //fields.Add( "ManufacturerName" );
                        break;
                    }
                case 2:
                    {
                        //2: azonosito novekvo,
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("ProductId");
                        //fields.Add( "ProductId" );
                        break;
                    }
                case 3:
                    {
                        //3: azonosito csokkeno,
                        builder = MongoDB.Driver.Builders.SortBy.Descending("ProductId");
                        //fields.Add( "ProductId" );
                        break;
                    }
                case 4:
                    {
                        //4 nev novekvo,
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("ItemName");
                        //fields.Add( "ItemName" );
                        break;
                    }
                case 5:
                    {
                        //5: nev csokkeno,
                        builder = MongoDB.Driver.Builders.SortBy.Descending("ItemName");
                        //fields.Add( "ItemName" );
                        break;
                    }
                case 6:
                    {
                        //6: ar novekvo, 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("Price2");
                        //fields.Add( "Price2" );
                        break;
                    }
                case 7:
                    {
                        //7 ar csokkeno
                        builder = MongoDB.Driver.Builders.SortBy.Descending("Price2");
                        //fields.Add( "Price2" );
                        break;
                    }
                case 8:
                    {
                        //8: jelleg1 + jelleg2 + jelleg3 + gyarto novekvo 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("Category1Name").Ascending("Category2Name").Ascending("Category3Name");
                        //fields.Add( "Category1Name" );
                        break;
                    }
                case 9:
                    {
                        //9: jelleg1 + jelleg2 + jelleg3 + gyarto csokkeno
                        builder = MongoDB.Driver.Builders.SortBy.Descending("Category1Name").Descending("Category2Name").Descending("Category3Name");
                        //fields.Add( "Category1Name" );
                        break;
                    }
                case 10:
                    {
                        //10: jelleg2 + jelleg3 + jelleg1 + gyarto novekvo 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("Category2Name").Ascending("Category3Name");
                        //fields.Add( "Category2Name" );
                        break;
                    }
                case 11:
                    {
                        //11: jelleg2 + jelleg3 + jelleg1 + gyarto csokkeno
                        builder = MongoDB.Driver.Builders.SortBy.Descending("Category2Name").Descending("Category3Name");
                        //fields.Add( "Category2Name" );
                        break;
                    }
                case 12:
                    {
                        //12: jelleg3 + jelleg2 + jelleg1 + gyarto novekvo 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("Category3Name");
                        //fields.Add( "Category3Name" );
                        break;
                    }
                case 13:
                    {
                        //13: jelleg3 + jelleg2 + jelleg1 + gyarto csokkeno
                        builder = MongoDB.Driver.Builders.SortBy.Descending("Category3Name");
                        //fields.Add( "Category3Name" );
                        break;
                    }
                case 14:
                    {
                        //14 : belső készlet növekvő, 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("HrpInnerStock");
                        //fields.Add( "HrpInnerStock" );
                        break;
                    }
                case 15:
                    {
                        //15 : belső készlet csökkenő,
                        builder = MongoDB.Driver.Builders.SortBy.Descending("HrpInnerStock");
                        //fields.Add("HrpInnerStock");
                        break;
                    }
                case 16:
                    {
                        //16 : külső készlet növekvő, 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("SerbianStock");
                        //fields.Add( "SerbianStock" );
                        break;
                    }
                case 17:
                    {
                        //17 : külső készlet csökkenő
                        builder = MongoDB.Driver.Builders.SortBy.Descending("SerbianStock");
                        //fields.Add( "SerbianStock" );
                        break;
                    }
                case 18:
                    {
                        //18 : készlet növekvő, 
                        builder = MongoDB.Driver.Builders.SortBy.Ascending("HrpInnerStock");
                        //fields.Add("HrpInnerStock");
                        break;
                    }
                case 19:
                    {
                        //19 : készlet csökkenő 
                        builder = MongoDB.Driver.Builders.SortBy.Descending("HrpInnerStock");
                        //fields.Add("HrpInnerStock");
                        break;
                    }
                default:
                    builder = MongoDB.Driver.Builders.SortBy.Ascending("ManufacturerName");
                    break;
            }
            return builder;
        }

        #endregion

        #region "katalógus elem"

        /// <summary>
        /// katalógus elem lekérdezése vállalatkód és cikkazonosíó alapján  
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Shared.Web.NoSql.Entities.CatalogueItem GetProductCatalogueItem(string dataAreaId, string productId)
        {
            try
            {
                ConnectToDatabase();

                //QueryDocument query = new QueryDocument("ProductId", productId);

                bool isCommonCompany = !(dataAreaId.ToLower().Equals(DataAreaIdSerbia));

                MongoDB.Driver.Builders.QueryComplete queryComplete = isCommonCompany ? MongoDB.Driver.Builders.Query.NE("DataAreaId", DataAreaIdSerbia) : MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId);

                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.LT("ItemState", 2));

                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("ProductId", productId));

                MongoCollection<Shared.Web.NoSql.Entities.CatalogueItem> collection = this.GetCollection();

                Shared.Web.NoSql.Entities.CatalogueItem item = collection.FindOne(queryComplete);

                //foreach (BsonDocument item in filteredList)
                //{
                //    resultList.Add(
                //       new Shared.Web.NoSql.Entities.CatalogueItem()
                //       {
                //           Action = item["Action"].AsBoolean,
                //           AgentItem = ConvertBsonDocumentToAgentItem(item["AgentItem"].AsBsonDocument),
                //           BargainCounter = item["BargainCounter"].AsBoolean,
                //           Category1Id = item["Category1Id"].AsString,
                //           Category1Name = item["Category1Name"].AsString,
                //           Category2Id = item["Category2Id"].AsString,
                //           Category2Name = item["Category2Name"].AsString,
                //           Category3Id = item["Category3Id"].AsString,
                //           Category3Name = item["Category3Name"].AsString,
                //           Description = item["Description"].AsString,
                //           FocusWeek = item["FocusWeek"].AsBoolean,
                //           Garanty = item["Garanty"].AsString,
                //           GarantyMode = item["GarantyMode"].AsString,
                //           HungarianStock = item["HungarianStock"].AsInt32,
                //           ItemName = item["ItemName"].AsString,
                //           ItemState = item["ItemState"].AsInt32,
                //           ManufacturerId = item["ManufacturerId"].AsString,
                //           ManufacturerName = item["ManufacturerName"].AsString,
                //           New = item["New"].AsBoolean,
                //           SerbianStock = item["SerbianStock"].AsInt32,
                //           PartNumber = item["PartNumber"].AsString,
                //           ProductId = item["ProductId"].AsString,
                //           Top10 = item["Top10"].AsBoolean,
                //           Price1 = item["Price1"].AsDouble,
                //           Price2 = item["Price2"].AsDouble,
                //           Price3 = item["Price3"].AsDouble,
                //           Price4 = item["Price4"].AsDouble,
                //           Price5 = item["Price5"].AsDouble,
                //           Price6 = item["Price6"].AsDouble,
                //           Pictures = ConvertBsonArrayToPictureList(item["Pictures"].AsBsonArray)
                //       }
                //    );
                //}
                return ( item == null ) ? new Shared.Web.NoSql.Entities.CatalogueItem() : item;
            }
            catch
            {
                return new Shared.Web.NoSql.Entities.CatalogueItem();
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        #endregion

        //private System.Web.UI.WebControls.SortDirection GetSortDirection(int sequence)
        //{
        //    return (new List<int> { 1, 3, 5, 7, 9, 11, 12, 15, 17, 19 }.Contains(sequence)) ? System.Web.UI.WebControls.SortDirection.Descending : System.Web.UI.WebControls.SortDirection.Ascending;
        //}

        #region "termékstruktúra"

        /// <summary>
        /// termékstruktúra lekérdezés
        /// </summary>
        /// <example>
        /// var query = Query.And(
        /// Query.EQ("author", "Kurt Vonnegut"),
        /// Query.EQ("title", "Cats Craddle") );
        /// </example>
        /// <param name="dataAreaId"></param>
        /// <param name="actionFilter"></param>
        /// <param name="bargainFilter"></param>
        /// <param name="focusweekFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="top10Filter"></param>
        /// <param name="textFilter"></param>
        /// <returns></returns>
        public List<Shared.Web.NoSql.Entities.Structure> GetProductStructure( string dataAreaId, 
                                                                              bool actionFilter, 
                                                                              bool bargainFilter, 
                                                                              bool focusweekFilter,
                                                                              bool newFilter, 
                                                                              bool stockFilter, 
                                                                              bool top10Filter, 
                                                                              string textFilter )
        {
            try
            {
                ConnectToDatabase();

                MongoDB.Driver.Builders.QueryComplete query = this.ConstructQueryDocument(String.Empty, String.Empty, String.Empty, String.Empty, 
                                                                                          actionFilter, 
                                                                                          bargainFilter, 
                                                                                          focusweekFilter, 
                                                                                          newFilter, 
                                                                                          stockFilter, 
                                                                                          top10Filter,
                                                                                          textFilter, 
                                                                                          dataAreaId);

                string[] sortFields = new string[] { "ManufacturerName", "Category1Name", "Category2Name", "Category3Name" };

                string[] fields = new string[] { "ManufacturerId", "ManufacturerName", "Category1Id", "Category1Name", "Category2Id", "Category2Name", "Category3Id", "Category3Name" };

                MongoCollection<BsonDocument> catalogue = this.Database.GetCollection("Catalogue");

                IEqualityComparer<Shared.Web.NoSql.Entities.CatalogueItem> catalogueComparer = new CatalogueComparer();

                MongoCursor<BsonDocument> filteredList = catalogue.Find(query).SetSortOrder(sortFields).SetFields(fields);
                    
                List<Shared.Web.NoSql.Entities.Structure> resultList = new List<Web.NoSql.Entities.Structure>();

                foreach (BsonDocument item in filteredList) 
                {
                    resultList.Add(
                       new Shared.Web.NoSql.Entities.Structure()
                       {
                           ManufacturerId = item["ManufacturerId"].IsString ? item["ManufacturerId"].AsString : String.Empty,
                           ManufacturerName = item["ManufacturerName"].IsString ? item["ManufacturerName"].AsString : String.Empty,
                           Category1Id = item["Category1Id"].IsString ? item["Category1Id"].AsString : String.Empty,
                           Category1Name = item["Category1Name"].IsString ? item["Category1Name"].AsString : String.Empty,
                           Category2Id = item["Category2Id"].IsString ? item["Category2Id"].AsString : String.Empty,
                           Category2Name = item["Category2Name"].IsString ? item["Category2Name"].AsString : String.Empty,
                           Category3Id = item["Category3Id"].IsString ? item["Category3Id"].AsString : String.Empty,
                           Category3Name = item["Category3Name"].IsString ? item["Category3Name"].AsString : String.Empty
                       }
                    );
                }
                return resultList;
            }
            catch
            {
                return new List<Shared.Web.NoSql.Entities.Structure>();
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        #endregion

        #region "leértékelt lista"

        /// <summary>
        /// leértékelt lista lekérdezése    
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public List<Shared.Web.NoSql.Entities.DiscountItem> GetDiscountList( string dataAreaId, 
                                                                             string currency )
        {
            try
            {
                ConnectToDatabase();

                MongoCollection<BsonDocument> catalogue = this.Database.GetCollection("Catalogue");

                //QueryDocument query = new QueryDocument("Action", true);

                MongoDB.Driver.Builders.QueryComplete queryComplete = ConstructQueryDocument(String.Empty, String.Empty, String.Empty, String.Empty, true, false, false, false, false, false, String.Empty, dataAreaId);

                List<Shared.Web.NoSql.Entities.DiscountItem> resultList = new List<Shared.Web.NoSql.Entities.DiscountItem>();

                string[] fields = new string[] { "ItemName", "PartNumber", "ProductId", "ManufacturerId", "Category1Id", "Category2Id", "Category3Id", "Price1", "Price2", "Price3", "Price4", "Price5", "Price6", "Pictures" };

                foreach (BsonDocument item in catalogue.Find(queryComplete).SetFields(fields).SetSortOrder(new string[] { "ItemName" })) 
                {
                    Shared.Web.NoSql.Entities.Picture picture = GetPrimaryPicture(item["Pictures"].AsBsonArray);

                    if (picture.RecId > 0)
                    {
                        resultList.Add(
                           new Shared.Web.NoSql.Entities.DiscountItem()
                           {
                               Name = item["ItemName"].IsString ? item["ItemName"].AsString : String.Empty,
                               PartNumber = item["PartNumber"].IsString ? item["PartNumber"].AsString : String.Empty,
                               ProductId = item["ProductId"].IsString ? item["ProductId"].AsString : String.Empty,
                               ManufacturerId = item["ManufacturerId"].IsString ? item["ManufacturerId"].AsString : String.Empty,
                               Category1Id = item["Category1Id"].IsString ? item["Category1Id"].AsString : String.Empty,
                               Category2Id = item["Category2Id"].IsString ? item["Category2Id"].AsString : String.Empty,
                               Category3Id = item["Category3Id"].IsString ? item["Category3Id"].AsString : String.Empty,
                               Price1 = item["Price1"].IsDouble ? item["Price1"].AsDouble : 0,
                               Price2 = item["Price2"].IsDouble ? item["Price2"].AsDouble : 0,
                               Price3 = item["Price3"].IsDouble ? item["Price3"].AsDouble : 0,
                               Price4 = item["Price4"].IsDouble ? item["Price4"].AsDouble : 0,
                               Price5 = item["Price5"].IsDouble ? item["Price5"].AsDouble : 0,
                               Price6 = item["Price6"].IsDouble ? item["Price6"].AsDouble : 0,
                               RecId = picture.RecId,
                               FileName = picture.FileName
                           }
                        );
                    }
                }

                int itemCount = resultList.Count > 100 ? 100 : resultList.Count;

                return resultList.Take(itemCount).ToList();
            }
            catch
            {
                return new List<Shared.Web.NoSql.Entities.DiscountItem>();
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        #endregion

        #endregion

        #region "nosql adatok karbantartása"

        /// <summary>
        /// cikkek betöltése nosql dokumentum kollekcióba
        /// </summary>
        /// <param name="list">nosql katalóguslista, ez a lista tartalmazza aa dokumentumkollekcióba mentendő elemeket</param>
        /// <returns>betöltött dokumentumok száma</returns>
        public long InsertCatalogueCollection(List<Shared.Web.NoSql.Entities.CatalogueItem> list)
        {
            try
            {
                ConnectToDatabase();

                MongoCollection<Shared.Web.NoSql.Entities.CatalogueItem> collection = this.GetCollection();

                //this.Database.RequestStart();

                //this.Database.RequestDone();

                collection.InsertBatch(list);

                return collection.Count();
            }
            catch
            {
                return 0;
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        /// <summary>
        /// paraméter kollekció elemeivel nosql adatok frissítése 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int UpdateCatalogueCollection(List<Shared.Web.NoSql.Entities.CatalogueItem> list)
        { 
            try
            {
                ConnectToDatabase();

                MongoCollection<Shared.Web.NoSql.Entities.CatalogueItem> collection = this.GetCollection();

                list.ForEach(delegate(Shared.Web.NoSql.Entities.CatalogueItem item) 
                {
                    var query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("ProductId", item.ProductId),
                                                                  MongoDB.Driver.Builders.Query.EQ("DataAreaId", item.DataAreaId));

                    Shared.Web.NoSql.Entities.CatalogueItem catalogueItem = collection.FindOne(query);

                    if (catalogueItem != null)
                    {
                        catalogueItem.ProductId = item.ProductId;
                        catalogueItem.PartNumber = item.PartNumber;
                        catalogueItem.ItemName = item.ItemName;
                        catalogueItem.ManufacturerId = item.ManufacturerId;
                        catalogueItem.ManufacturerName = item.ManufacturerName;
                        catalogueItem.ManufacturerNameEnglish = item.ManufacturerNameEnglish;
                        catalogueItem.Category1Id = item.Category1Id;
                        catalogueItem.Category1Name = item.Category1Name;
                        catalogueItem.Category1NameEnglish = item.Category1NameEnglish;
                        catalogueItem.Category2Id = item.Category2Id;
                        catalogueItem.Category2Name = item.Category2Name;
                        catalogueItem.Category2NameEnglish = item.Category2NameEnglish;
                        catalogueItem.Category3Id = item.Category3Id;
                        catalogueItem.Category3Name = item.Category3Name;
                        catalogueItem.Category3NameEnglish = item.Category3NameEnglish;
                        catalogueItem.Garanty = item.Garanty;
                        catalogueItem.GarantyMode = item.GarantyMode;
                        catalogueItem.ItemName = item.ItemName;
                        catalogueItem.ItemNameEnglish = item.ItemNameEnglish;
                        catalogueItem.Action = item.Action;
                        catalogueItem.BargainCounter = item.BargainCounter;
                        catalogueItem.Top10 = item.Top10;
                        catalogueItem.FocusWeek = item.FocusWeek;
                        catalogueItem.New = item.New;
                        catalogueItem.Description = item.Description;
                        catalogueItem.DescriptionEnglish = item.DescriptionEnglish;
                        catalogueItem.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        catalogueItem.CreatedTime = DateTime.UtcNow.Minute;
                        catalogueItem.ModifiedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        catalogueItem.ModifiedTime = DateTime.UtcNow.Minute;
                        catalogueItem.Price1 = item.Price1;
                        catalogueItem.Price2 = item.Price2;
                        catalogueItem.Price3 = item.Price3;
                        catalogueItem.Price4 = item.Price4;
                        catalogueItem.Price5 = item.Price5;
                        catalogueItem.Price6 = item.Price6;
                        catalogueItem.Currency = item.Currency;
                        catalogueItem.DataAreaId = item.DataAreaId;
                        catalogueItem.HrpInnerStock = item.HrpInnerStock;
                        catalogueItem.HrpOuterStock = item.HrpOuterStock;
                        catalogueItem.BscInnerStock = item.BscInnerStock;
                        catalogueItem.BscOuterStock = item.BscOuterStock;
                        collection.Save<Shared.Web.NoSql.Entities.CatalogueItem>(catalogueItem);
                    }
                    else
                    {
                        //collection.Insert<Shared.Web.NoSql.Entities.CatalogueItem>(item);
                    }
                });

                return 1;
            }
            catch
            {
                return -2;
            }
            finally
            {
                DisconnectFromDatabase();
            }            
        }

        /// <summary>
        /// egyetlen cikk elemet NoSql kollekcióban módosító, vagy felvevő metódus
        /// </summary>
        /// <param name="item"></param>
        public void InsertOrUpdateCatalogueItem(Shared.Web.NoSql.Entities.CatalogueItem item, int syncOperation)
        {
            try
            {
                ConnectToDatabase();

                MongoCollection<Shared.Web.NoSql.Entities.CatalogueItem> collection = this.GetCollection();

                var query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("ProductId", item.ProductId),
                                                                MongoDB.Driver.Builders.Query.EQ("DataAreaId", item.DataAreaId));

                Shared.Web.NoSql.Entities.CatalogueItem catalogueItem = collection.FindOne(query);

                if (catalogueItem != null && syncOperation.Equals(2))
                {
                    catalogueItem.ProductId = item.ProductId;
                    catalogueItem.PartNumber = item.PartNumber;
                    catalogueItem.ItemName = item.ItemName;
                    catalogueItem.ManufacturerId = item.ManufacturerId;
                    catalogueItem.ManufacturerName = item.ManufacturerName;
                    catalogueItem.ManufacturerNameEnglish = item.ManufacturerNameEnglish;
                    catalogueItem.Category1Id = item.Category1Id;
                    catalogueItem.Category1Name = item.Category1Name;
                    catalogueItem.Category1NameEnglish = item.Category1NameEnglish;
                    catalogueItem.Category2Id = item.Category2Id;
                    catalogueItem.Category2Name = item.Category2Name;
                    catalogueItem.Category2NameEnglish = item.Category2NameEnglish;
                    catalogueItem.Category3Id = item.Category3Id;
                    catalogueItem.Category3Name = item.Category3Name;
                    catalogueItem.Category3NameEnglish = item.Category3NameEnglish;
                    catalogueItem.Garanty = item.Garanty;
                    catalogueItem.GarantyMode = item.GarantyMode;
                    catalogueItem.ItemName = item.ItemName;
                    catalogueItem.ItemNameEnglish = item.ItemNameEnglish;
                    catalogueItem.Action = item.Action;
                    catalogueItem.BargainCounter = item.BargainCounter;
                    catalogueItem.Top10 = item.Top10;
                    catalogueItem.FocusWeek = item.FocusWeek;
                    catalogueItem.New = item.New;
                    catalogueItem.Description = item.Description;
                    catalogueItem.DescriptionEnglish = item.DescriptionEnglish;
                    catalogueItem.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    catalogueItem.CreatedTime = DateTime.UtcNow.Minute;
                    catalogueItem.ModifiedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    catalogueItem.ModifiedTime = DateTime.UtcNow.Minute;
                    catalogueItem.Price1 = item.Price1;
                    catalogueItem.Price2 = item.Price2;
                    catalogueItem.Price3 = item.Price3;
                    catalogueItem.Price4 = item.Price4;
                    catalogueItem.Price5 = item.Price5;
                    catalogueItem.Price6 = item.Price6;
                    catalogueItem.Currency = item.Currency;
                    catalogueItem.DataAreaId = item.DataAreaId;
                    catalogueItem.HrpInnerStock = item.HrpInnerStock;
                    catalogueItem.HrpOuterStock = item.HrpOuterStock;
                    catalogueItem.BscInnerStock = item.BscInnerStock;
                    catalogueItem.BscOuterStock = item.BscOuterStock;
                    collection.Save<Shared.Web.NoSql.Entities.CatalogueItem>(catalogueItem);
                }
                else if ( syncOperation.Equals(1) )
                {
                    catalogueItem.ProductId = item.ProductId;
                    catalogueItem.PartNumber = item.PartNumber;
                    catalogueItem.ItemName = item.ItemName;
                    catalogueItem.ItemNameEnglish = item.ItemNameEnglish;
                    catalogueItem.ManufacturerId = item.ManufacturerId;
                    catalogueItem.ManufacturerName = item.ManufacturerName;
                    catalogueItem.ManufacturerNameEnglish = item.ManufacturerNameEnglish;
                    catalogueItem.Category1Id = item.Category1Id;
                    catalogueItem.Category1Name = item.Category1Name;
                    catalogueItem.Category1NameEnglish = item.Category1NameEnglish;
                    catalogueItem.Category2Id = item.Category2Id;
                    catalogueItem.Category2Name = item.Category2Name;
                    catalogueItem.Category2NameEnglish = item.Category2NameEnglish;
                    catalogueItem.Category3Id = item.Category3Id;
                    catalogueItem.Category3Name = item.Category3Name;
                    catalogueItem.Category3NameEnglish = item.Category3NameEnglish;
                    catalogueItem.Garanty = item.Garanty;
                    catalogueItem.GarantyMode = item.GarantyMode;
                    catalogueItem.ItemName = item.ItemName;
                    catalogueItem.Action = item.Action;
                    catalogueItem.BargainCounter = item.BargainCounter;
                    catalogueItem.Top10 = item.Top10;
                    catalogueItem.FocusWeek = item.FocusWeek;
                    catalogueItem.New = item.New;
                    catalogueItem.Description = item.Description;
                    catalogueItem.DescriptionEnglish = item.DescriptionEnglish;
                    catalogueItem.CreatedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    catalogueItem.CreatedTime = DateTime.UtcNow.Minute;
                    catalogueItem.ModifiedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    catalogueItem.ModifiedTime = DateTime.UtcNow.Minute;
                    catalogueItem.Price1 = item.Price1;
                    catalogueItem.Price2 = item.Price2;
                    catalogueItem.Price3 = item.Price3;
                    catalogueItem.Price4 = item.Price4;
                    catalogueItem.Price5 = item.Price5;
                    catalogueItem.Price6 = item.Price6;
                    catalogueItem.Currency = item.Currency;
                    catalogueItem.DataAreaId = item.DataAreaId;
                    catalogueItem.HrpInnerStock = item.HrpInnerStock;
                    catalogueItem.HrpOuterStock = item.HrpOuterStock;
                    catalogueItem.BscInnerStock = item.BscInnerStock;
                    catalogueItem.BscOuterStock = item.BscOuterStock;
                    collection.Insert<Shared.Web.NoSql.Entities.CatalogueItem>(catalogueItem);
                }
            }
            catch
            {
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        public void DeleteCatalogueItem(string productId, string dataAreaId)
        {
            try
            {
                ConnectToDatabase();

                MongoCollection<Shared.Web.NoSql.Entities.CatalogueItem> collection = this.GetCollection();

                var query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("ProductId", productId),
                                                              MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId));

                Shared.Web.NoSql.Entities.CatalogueItem catalogueItem = collection.FindOne(query);

                if (catalogueItem != null) 
                { 
                    collection.Remove(query); 
                }
            }
            catch
            {
            }
            finally
            {
                DisconnectFromDatabase();
            }            
        }

        public bool ClearCollection(string dataAreaId)
        {
            try
            {
                ConnectToDatabase();

                IMongoQuery query = MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId);

                SafeModeResult res = this.GetCollection().Remove(query);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                DisconnectFromDatabase();
            }        
        }

        /// <summary>
        /// cikkek kollekció törlése
        /// </summary>
        /// <returns></returns>
        public new bool DropCollection()
        { 
            try
            {
                ConnectToDatabase();

                base.DropCollection();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                DisconnectFromDatabase();
            }    
        }

        #endregion

        #region "képek lista lekérdezése"

        public List<Shared.Web.NoSql.Entities.Picture> ProductPictureList(string dataAreaId, string productId, bool primary)
        {
            try
            {
                ConnectToDatabase();

                bool isCommonCompany = !(dataAreaId.ToLower().Equals(DataAreaIdSerbia));

                MongoDB.Driver.Builders.QueryComplete queryComplete = isCommonCompany ? MongoDB.Driver.Builders.Query.NE("DataAreaId", DataAreaIdSerbia) : MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId);

                queryComplete = MongoDB.Driver.Builders.Query.And(queryComplete, MongoDB.Driver.Builders.Query.EQ("ProductId", productId));

                MongoCollection<Shared.Web.NoSql.Entities.CatalogueItem> collection = this.GetCollection();

                Shared.Web.NoSql.Entities.CatalogueItem item = collection.FindOne(queryComplete);

                if (item == null)
                { 
                    return new List<Shared.Web.NoSql.Entities.Picture>();
                }

                return item.Pictures;
            }
            catch
            {
                return new List<Shared.Web.NoSql.Entities.Picture>();
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// Func<int, int, bool> f = (x, y) => x == y;
    //var comparer = new Comparer<int>(f);
    //Console.WriteLine(comparer.Equals(1, 1));
    //Console.WriteLine(comparer.Equals(1, 2));

    /// </example>
    /// <typeparam name="T"></typeparam>
    class Comparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;

        public Comparer(Func<T, T, bool> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            _comparer = comparer;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }
    }

    /// <summary>
    /// IEqualityComparer<Contact> customComparer = new ContactEmailComparer();
    /// IEnumerable<Contact> distinctEmails = collection.Distinct(customComparer); 
    /// </summary>
    class CatalogueComparer : IEqualityComparer<Shared.Web.NoSql.Entities.CatalogueItem>
    {
        #region IEqualityComparer<Shared.Web.NoSql.Entities.Catalogue> Members

        public bool Equals(Shared.Web.NoSql.Entities.CatalogueItem x, Shared.Web.NoSql.Entities.CatalogueItem y)
        {
            return x.ManufacturerId.Equals(y.ManufacturerId) && x.Category1Id.Equals(y.Category1Id) && x.Category2Id.Equals(y.Category2Id) && x.Category3Id.Equals(y.Category3Id);
        }

        public int GetHashCode(Shared.Web.NoSql.Entities.CatalogueItem obj)
        {
            return obj.ManufacturerId.GetHashCode();
        }

        #endregion
    }

}

