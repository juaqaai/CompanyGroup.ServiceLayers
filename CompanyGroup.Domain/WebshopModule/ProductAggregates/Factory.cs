using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// model factory class
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// termék létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        //public static CompanyGroup.Domain.WebshopModule.Product CreateProduct(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        //{
        //    CompanyGroup.Domain.WebshopModule.Product product = new CompanyGroup.Domain.WebshopModule.Product();
        //    //kereskedelmi készletről nem vásárolható meg
        //    product.Available = innerProduct.ItemState < 2;
        //    product.AverageInventory = innerProduct.AverageInventory;
        //    //product.CannotCancel
        //    product.CreatedDate = innerProduct.CreatedDate;
        //    product.CreatedTime = innerProduct.CreatedTime;
        //    //product.Comparable
        //    //product.CustomerPrice
        //    product.DataAreaId = innerProduct.DataAreaId;
        //    product.Description = innerProduct.Description;
        //    product.DescriptionEnglish = innerProduct.DescriptionEnglish;
        //    product.Discount = innerProduct.Discount;
        //    product.Garanty = CreateGaranty(innerProduct);
        //    product.Id = MongoDB.Bson.ObjectId.Empty;
        //    //product.IsInCart
        //    //product.IsInNewsletter
        //    //product.IsInStock
        //    product.ItemState = CompanyGroup.Domain.Core.Adapter.ConvertItemStateIntToEnum(innerProduct.ItemState);
        //    product.ModifiedDate = innerProduct.ModifiedDate;
        //    product.ModifiedTime = innerProduct.ModifiedTime;
        //    product.New = innerProduct.New; 
        //    product.PartNumber = innerProduct.PartNumber;
        //    product.Pictures = CreatePictures(innerProduct);
        //    product.Prices = CreatePrices(innerProduct);
        //    //product.PrimaryPicture
        //    product.ProductId = innerProduct.ProductId;
        //    //product.ProductManager = CreateProductManager(innerProduct);
        //    product.ProductName = innerProduct.ItemName;
        //    product.ProductNameEnglish = innerProduct.ItemNameEnglish;
        //    product.SecondHandList = CreateSecondHandList(innerProduct.SecondHandList);
        //    //product.SequenceNumber
        //    product.ShippingDate = innerProduct.ShippingDate;
        //    product.StandardConfigId = innerProduct.StandardConfigId;
        //    product.Stock = CreateStock(innerProduct);
        //    product.Structure = CreateStructure(innerProduct);
        //    return product;
        //}

        /// <summary>
        /// struktúra létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Structure CreateStructure(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            return new CompanyGroup.Domain.WebshopModule.Structure()
            {
                Category1 = CreateCategory1(innerProduct),
                Category2 = CreateCategory2(innerProduct),
                Category3 = CreateCategory3(innerProduct),
                Manufacturer = CreateManufacturer(innerProduct)
            };
        }

        /// <summary>
        /// struktúra létrehozása
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="manufacturerName"></param>
        /// <param name="category1Id"></param>
        /// <param name="category1Name"></param>
        /// <param name="category2Id"></param>
        /// <param name="category2Name"></param>
        /// <param name="category3Id"></param>
        /// <param name="category3Name"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Structure CreateStructure(string manufacturerId, string manufacturerName,
                                                                                  string category1Id, string category1Name,
                                                                                  string category2Id, string category2Name,
                                                                                  string category3Id, string category3Name)
        {
            return new CompanyGroup.Domain.WebshopModule.Structure()
            {
                Manufacturer = CreateManufacturer(manufacturerId, manufacturerName, manufacturerName),
                Category1 = CreateCategory(category1Id, category1Name, category1Name),
                Category2 = CreateCategory(category2Id, category2Name, category2Name),
                Category3 = CreateCategory(category3Id, category3Name, category3Name)
            };
        }

        /// <summary>
        /// gyártó létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Manufacturer CreateManufacturer(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            return new CompanyGroup.Domain.WebshopModule.Manufacturer() { ManufacturerId = innerProduct.ManufacturerId, ManufacturerName = innerProduct.ManufacturerName, ManufacturerEnglishName = innerProduct.ManufacturerNameEnglish };
        }

        /// <summary>
        /// gyártó létrehozása
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="manufacturerName"></param>
        /// <param name="manufacturerNameEnglish"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Manufacturer CreateManufacturer(string manufacturerId, string manufacturerName, string manufacturerNameEnglish)
        {
            return new CompanyGroup.Domain.WebshopModule.Manufacturer() { ManufacturerId = manufacturerId, ManufacturerName = manufacturerName, ManufacturerEnglishName = manufacturerNameEnglish };
        }

        /// <summary>
        /// kategória létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Category CreateCategory1(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            return new CompanyGroup.Domain.WebshopModule.Category() { CategoryId = innerProduct.Category1Id, CategoryName = innerProduct.Category1Name, CategoryEnglishName = innerProduct.Category1NameEnglish };
        }

        /// <summary>
        /// kategória létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Category CreateCategory2(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            return new CompanyGroup.Domain.WebshopModule.Category() { CategoryId = innerProduct.Category2Id, CategoryName = innerProduct.Category2Name, CategoryEnglishName = innerProduct.Category2NameEnglish };
        }

        /// <summary>
        /// kategória létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Category CreateCategory3(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            return new CompanyGroup.Domain.WebshopModule.Category() { CategoryId = innerProduct.Category3Id, CategoryName = innerProduct.Category3Name, CategoryEnglishName = innerProduct.Category3NameEnglish };
        }

        /// <summary>
        /// kategória létrehozása
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryName"></param>
        /// <param name="categoryNameEnglish"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Category CreateCategory(string categoryId, string categoryName, string categoryNameEnglish)
        {
            return new CompanyGroup.Domain.WebshopModule.Category() { CategoryId = categoryId, CategoryName = categoryName, CategoryEnglishName = categoryNameEnglish };
        }

        /// <summary>
        /// flagek létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        //public static CompanyGroup.Domain.WebshopModule.Flags CreateFlags(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        //{
        //    bool inStock = false;

        //    inStock = (innerProduct.DataAreaId.Equals(DataAreaId.Serbian)) ? (innerProduct.InnerStock + innerProduct.OuterStock + innerProduct.SerbianStock) > 0 : (innerProduct.InnerStock + innerProduct.OuterStock) > 0;

        //    bool isInNewsletter = false;

        //    return new CompanyGroup.Domain.WebshopModule.Flags()
        //    {
        //        Action = innerProduct.Discount, 
        //        New = innerProduct.New,
        //        InStock = inStock,
        //        IsInNewsletter = isInNewsletter
        //    };
        //}

        /// <summary>
        /// készlet objektum létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Stock CreateStock(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            return new CompanyGroup.Domain.WebshopModule.Stock()
            {
                Inner = innerProduct.InnerStock,
                Outer = innerProduct.OuterStock
            };
        }

        /// <summary>
        /// ár objektum létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Prices CreatePrices(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            return new CompanyGroup.Domain.WebshopModule.Prices()
            {
                Price1 = innerProduct.Price1,
                Price2 = innerProduct.Price2,
                Price3 = innerProduct.Price3,
                Price4 = innerProduct.Price4,
                Price5 = innerProduct.Price5,
                Currency = innerProduct.Currency
            };        
        }

        /// <summary>
        /// gerancia objektum létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Garanty CreateGaranty(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            return new CompanyGroup.Domain.WebshopModule.Garanty() { Mode = innerProduct.GarantyMode, Time = innerProduct.GarantyTime };
        }

        /// <summary>
        /// termékmanager objektum létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        //public static CompanyGroup.Domain.PartnerModule.ProductManager CreateProductManager(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        //{
        //    return new CompanyGroup.Domain.PartnerModule.ProductManager()
        //    {
        //        EmployeeId = innerProduct.ProductManager.EmployeeId,
        //        Name = innerProduct.ProductManager.Name,
        //        Email = innerProduct.ProductManager.Email,
        //        Extension = innerProduct.ProductManager.Extension,
        //        Mobile = innerProduct.ProductManager.Mobile
        //    };        
        //}

        /// <summary>
        /// termék képeinek létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Pictures CreatePictures(CompanyGroup.Domain.MaintainModule.Product innerProduct)
        {
            CompanyGroup.Domain.WebshopModule.Pictures pictures = new CompanyGroup.Domain.WebshopModule.Pictures();

            pictures.AddRange(innerProduct.Pictures.ConvertAll(x => CreatePicture(x)));

            return pictures;
        }

        /// <summary>
        /// kép objektum létrehozása
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Picture CreatePicture(CompanyGroup.Domain.MaintainModule.Picture picture)
        {
            return new CompanyGroup.Domain.WebshopModule.Picture() { FileName = picture.FileName, Primary = picture.Primary, RecId = picture.RecId };
        }

        /// <summary>
        /// kép objektum létrehozása
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="primary"></param>
        /// <param name="recId"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.Picture CreatePicture(string fileName, bool primary, long recId)
        {
            return new CompanyGroup.Domain.WebshopModule.Picture() { FileName = fileName, Primary = primary, RecId = recId };
        }

        /// <summary>
        /// leértékelt cikkek lista létrehozása
        /// </summary>
        /// <param name="innerProduct"></param>
        /// <returns></returns>
        //public static CompanyGroup.Domain.WebshopModule.SecondHandList CreateSecondHandList(List<CompanyGroup.Domain.MaintainModule.SecondHand> from)
        //{
        //    CompanyGroup.Domain.WebshopModule.SecondHandList secondHandList = new CompanyGroup.Domain.WebshopModule.SecondHandList();

        //    secondHandList.AddRange(from.ConvertAll(x => CreateSecondHand(x)));

        //    return secondHandList;
        //}

        /// <summary>
        /// leértékelt cikk létrehozása
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="inventLocationId"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="statusDescription"></param>
        /// <returns></returns>
        //public static CompanyGroup.Domain.WebshopModule.SecondHand CreateSecondHand(CompanyGroup.Domain.MaintainModule.SecondHand secondHand)
        //{
        //    return new CompanyGroup.Domain.WebshopModule.SecondHand(secondHand.ConfigId, secondHand.InventLocationId, secondHand.Quantity, secondHand.Price, secondHand.StatusDescription);
        //}

        /// <summary>
        /// felhasználói kosár létrehozása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="visitorId"></param>
        /// <param name="companyId"></param>
        /// <param name="personId"></param>
        /// <param name="name"></param>
        /// <param name="currency"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static CompanyGroup.Domain.WebshopModule.ShoppingCart CreateShoppingCart(int id, string visitorId, string companyId, string personId, string name, string currency, bool active)
        {
            return new CompanyGroup.Domain.WebshopModule.ShoppingCart(id, visitorId, companyId, personId, name, currency, active);
        }
    }
}
