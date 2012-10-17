
namespace Shared.Web.DataAccess.NoSql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface ICatalogueDb
    {
        bool DeleteIndexes();

        bool CreateIndexes();

        List<Shared.Web.NoSql.Entities.Structure> GetProductStructure( string dataAreaId, 
                                                                       bool actionFilter, 
                                                                       bool bargainFilter, 
                                                                       bool focusweekFilter,
                                                                       bool newFilter, 
                                                                       bool stockFilter, 
                                                                       bool top10Filter, 
                                                                       string textFilter );

        List<Shared.Web.NoSql.Entities.DiscountItem> GetDiscountList( string dataAreaId, string currency );

        List<Shared.Web.NoSql.Entities.CatalogueItem> GetProductCatalogue( string dataAreaId, 
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
                                                                           ref int count);

        Shared.Web.NoSql.Entities.CatalogueItem GetProductCatalogueItem(string dataAreaId, string productId);

        /// <summary>
        /// Cikkek betöltése nosql dokumentum kollekcióba 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        long InsertCatalogueCollection(List<Shared.Web.NoSql.Entities.CatalogueItem> list);

        int UpdateCatalogueCollection(List<Shared.Web.NoSql.Entities.CatalogueItem> list);

        void InsertOrUpdateCatalogueItem(Shared.Web.NoSql.Entities.CatalogueItem item, int syncOperation);

        void DeleteCatalogueItem(string productId, string dataAreaId);

        bool ClearCollection(string dataAreaId); 

        bool DropCollection();

        List<Shared.Web.NoSql.Entities.Picture> ProductPictureList(string dataAreaId, string productId, bool primary);

    }
}
