﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// webshop lista view-hoz tartozó típus, webshop catalogue controller index action JSonResult-ban                  
    /// </summary>
    public class Catalogue
    {
        public Catalogue(
            //CompanyGroup.Dto.WebshopModule.Structures structures, 
                         CompanyGroup.Dto.WebshopModule.Products products,
                         CompanyGroup.WebClient.Models.Visitor visitor,
                         CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart, 
                         List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedItems, 
                         List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedItems,
                         bool shoppingCartOpenStatus, 
                         bool catalogueOpenStatus, 
                         int sequence,
                         CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses,
                         CompanyGroup.Dto.WebshopModule.BannerList bannerList, 
                         CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions)
        {
            //this.Structures = new CompanyGroup.WebClient.Models.Structures(structures);

            this.Products = products;   

            this.Visitor = visitor;

            this.ActiveCart = activeCart;

            this.OpenedItems = openedItems;

            this.StoredItems = storedItems;

            this.Sequence = sequence;

            this.ShoppingCartOpenStatus = shoppingCartOpenStatus;

            this.CatalogueOpenStatus = catalogueOpenStatus;

            this.DeliveryAddresses = deliveryAddresses;

            this.BannerList = bannerList;

            this.LeasingOptions = leasingOptions;
        }

        //public CompanyGroup.WebClient.Models.Structures Structures { get; set; }

        public CompanyGroup.Dto.WebshopModule.Products Products { get; set; }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }

        public CompanyGroup.Dto.WebshopModule.ShoppingCart ActiveCart { get; set; } 

        public List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> OpenedItems { get; set; }

        public List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> StoredItems { get; set; } 

        public bool ShoppingCartOpenStatus { get; set; }

        public bool CatalogueOpenStatus { get; set; }

        /// <summary>
        /// van-e aktív szűrési feltétel beállítva, vagy nincs beállítva
        /// </summary>
        public bool IsActiveFilter { get; set; }

        public int Sequence { get; set; }

        public CompanyGroup.Dto.PartnerModule.DeliveryAddresses DeliveryAddresses { get; set; }

        public CompanyGroup.Dto.WebshopModule.BannerList BannerList { get; set; }

        public CompanyGroup.Dto.WebshopModule.LeasingOptions LeasingOptions { get; set; }

    }

}
