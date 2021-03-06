﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.MaintainModule
{
    /// <summary>
    /// NHibernate product repository, adminisztráció
    /// </summary>
    public class ProductRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.MaintainModule.IProductRepository
    {
        /// <summary>
        /// termék repository konstruktor
        /// </summary>
        /// <param name="settings"></param>
        public ProductRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// gyártólista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.Manufacturer> GetManufacturerList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_ManufacturerList").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.Manufacturer).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.Manufacturer>() as List<CompanyGroup.Domain.MaintainModule.Manufacturer>;
        }

        /// <summary>
        /// jelleg1 lista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.FirstLevelCategory> GetFirstLevelCategoryList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_Category1List").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.FirstLevelCategory).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.FirstLevelCategory>() as List<CompanyGroup.Domain.MaintainModule.FirstLevelCategory>;
        }

        /// <summary>
        /// jelleg2 lista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.SecondLevelCategory> GetSecondLevelCategoryList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_Category2List").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.SecondLevelCategory).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.SecondLevelCategory>() as List<CompanyGroup.Domain.MaintainModule.SecondLevelCategory>;
        }

        /// <summary>
        /// jelleg3 lista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.ThirdLevelCategory> GetThirdLevelCategoryList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_Category3List").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.ThirdLevelCategory).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.ThirdLevelCategory>() as List<CompanyGroup.Domain.MaintainModule.ThirdLevelCategory>;
        }

        /// <summary>
        /// termékmanager lista lekérdezése
        /// </summary>
        /// <returns></returns>
        //public List<CompanyGroup.Domain.MaintainModule.ProductManager> GetProductManagerList(string dataAreaId)
        //{
        //    NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_EmployeeList").SetString("DataAreaId", dataAreaId).SetResultTransformer(
        //                                    new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.ProductManager).GetConstructors()[0]));

        //    return query.List<CompanyGroup.Domain.MaintainModule.ProductManager>() as List<CompanyGroup.Domain.MaintainModule.ProductManager>;
        //}

        /// <summary>
        /// termékleírások lista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.ProductDescription> GetProductDescriptionList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_ProductDescriptionList").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.ProductDescription).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.ProductDescription>() as List<CompanyGroup.Domain.MaintainModule.ProductDescription>;
        }

        /// <summary>
        /// terméklista lekérdezése (a használt készleten lévő termékek kiszűrése megtörténik)
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.Product> GetProductList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_ProductList").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.Product).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.Product>() as List<CompanyGroup.Domain.MaintainModule.Product>;        
        }

        /// <summary>
        /// használt terméklista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.Product> GetSecondHandProductList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_SecondHandProductList").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.Product).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.Product>() as List<CompanyGroup.Domain.MaintainModule.Product>;
        }

        /// <summary>
        /// termékekhez tartozó képek lista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.Picture> GetPictureList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_PictureList").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.Picture).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.Picture>() as List<CompanyGroup.Domain.MaintainModule.Picture>;
        }

        /// <summary>
        /// termékekhez tartozó készlet lista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.Stock> GetStockList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_StockList").SetString("DataAreaId", dataAreaId)
                                                                                     .SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.Stock).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.Stock>() as List<CompanyGroup.Domain.MaintainModule.Stock>;
        }

        /// <summary>
        /// termékekhez tartozó leértékelt - használt - lista lekérdezése
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.SecondHand> GetSecondHandList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_SecondHandList").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.SecondHand).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.SecondHand>() as List<CompanyGroup.Domain.MaintainModule.SecondHand>;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.InventName> GetInventNameEnglishList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_InventNameEnglish").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.InventName).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.InventName>() as List<CompanyGroup.Domain.MaintainModule.InventName>;
        }


        /// <summary>
        /// beszerzési rendelés sorok kiolvasása
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.MaintainModule.PurchaseOrderLine> GetPurchaseOrderLineList(string dataAreaId)
        {
            NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_PurchaseOrderLine").SetString("DataAreaId", dataAreaId).SetResultTransformer(
                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.PurchaseOrderLine).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.MaintainModule.PurchaseOrderLine>() as List<CompanyGroup.Domain.MaintainModule.PurchaseOrderLine>;            
        }

    }
}
