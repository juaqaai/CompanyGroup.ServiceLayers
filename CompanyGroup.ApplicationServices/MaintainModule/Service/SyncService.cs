using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using CompanyGroup.Data.MaintainModule;
using System.Linq;
using NHibernate;
using CompanyGroup.Domain.MaintainModule;

namespace CompanyGroup.ApplicationServices.MaintainModule
{
    /// <summary>
    /// 
    /// </summary>
    public class SyncService : ISyncService
    {
        private CompanyGroup.Domain.MaintainModule.ISyncRepository syncRepository;

        private CompanyGroup.Domain.WebshopModule.IProductRepository productRepository;

        /// <summary>
        /// konstruktor repository interfész paraméterrel
        /// </summary>
        /// <param name="syncRepository"></param>
        /// <param name="productRepository"></param>
        public SyncService(CompanyGroup.Domain.MaintainModule.ISyncRepository syncRepository, CompanyGroup.Domain.WebshopModule.IProductRepository productRepository)
        {
            if (syncRepository == null)
            {
                throw new ArgumentNullException("SyncRepository");
            }

            if (productRepository == null)
            {
                throw new ArgumentNullException("ProductRepository");
            }

            this.syncRepository = syncRepository;

            this.productRepository = productRepository;
        }

        /// <summary>
        /// aktuális készlet frissítése
        /// </summary>
        /// <returns></returns>
        public void StockUpdate(CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require((request != null), "SyncService StockUpdate request cannot be null, or empty!");

                //aktuális készlet lekérdezése az ERP adatbázisból
                int stock = syncRepository.GetStockChange(request.DataAreaId, request.InventLocationId, request.ProductId);

                CompanyGroup.Domain.WebshopModule.CatalogueStockUpdate req = new CompanyGroup.Domain.WebshopModule.CatalogueStockUpdate(request.DataAreaId, request.ProductId, stock);

                //készlet darabszám befrissítése
                productRepository.StockUpdate(req);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
