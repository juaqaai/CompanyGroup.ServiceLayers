using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class WebshopApiController : ApiBaseController
    {
        // GET api/webshop/Catalogue
        /// <summary>
        /// katalógus lekérdezése, kiinduló állapot
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Catalogue")]
        public CompanyGroup.WebClient.Models.Catalogue Catalogue()
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //struktúrák lekérdezése
            CompanyGroup.Dto.ServiceRequest.GetAllStructure allStructure = new CompanyGroup.Dto.ServiceRequest.GetAllStructure()
            {
                ActionFilter = false,
                BargainFilter = false,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                HrpFilter = true,
                BscFilter = true,
                IsInNewsletterFilter = false,
                ManufacturerIdList = new List<string>(),
                NewFilter = false,
                StockFilter = false,
                TextFilter = String.Empty,
                PriceFilter = "0",
                PriceFilterRelation = "0",
                NameOrPartNumberFilter = ""
            };

            CompanyGroup.Dto.WebshopModule.Structures structures = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllStructure, CompanyGroup.Dto.WebshopModule.Structures>("Structure", "GetAll", allStructure);

            //katalógus lekérdezése
            CompanyGroup.Dto.ServiceRequest.GetAllProduct allProduct = new CompanyGroup.Dto.ServiceRequest.GetAllProduct()
            {
                ActionFilter = false,
                BargainFilter = false,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                Currency = visitorData.Currency,
                CurrentPageIndex = 1,
                HrpFilter = true,
                BscFilter = true,
                IsInNewsletterFilter = false,
                ItemsOnPage = 30,
                ManufacturerIdList = new List<string>(),
                NewFilter = false,
                Sequence = 0,
                StockFilter = false,
                TextFilter = String.Empty,
                PriceFilter = "0",
                PriceFilterRelation = "0",
                VisitorId = visitor.Id,
                NameOrPartNumberFilter = ""
            };

            CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllProduct, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetAll", allProduct);

            //banner lista lekérdezése
            CompanyGroup.Dto.ServiceRequest.GetBannerList bannerListRequest = new CompanyGroup.Dto.ServiceRequest.GetBannerList()
            {
                BscFilter = true,
                HrpFilter = true,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                Currency = visitorData.Currency,
                VisitorId = visitor.Id
            };

            CompanyGroup.Dto.WebshopModule.BannerList bannerList = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetBannerList, CompanyGroup.Dto.WebshopModule.BannerList>("Product", "GetBannerList", bannerListRequest);

            //kosár lekérdezések     
            bool shoppingCartOpenStatus = visitorData.IsShoppingCartOpened;

            bool catalogueOpenStatus = visitorData.IsCatalogueOpened;

            CompanyGroup.WebClient.Models.ShoppingCartInfo cartInfo = new CompanyGroup.WebClient.Models.ShoppingCartInfo();  //(visitor.IsValidLogin) ? this.GetCartInfo() : 

            if (visitor.IsValidLogin && !String.IsNullOrEmpty(visitorData.CartId))
            {
                CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo shoppingCartInfoRequest = new CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo(visitorData.CartId, visitor.Id);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "GetShoppingCartInfo", shoppingCartInfoRequest);

                cartInfo.ActiveCart = (shoppingCartInfo != null) ? shoppingCartInfo.ActiveCart : cartInfo.ActiveCart;
                cartInfo.OpenedItems = (shoppingCartInfo != null) ? shoppingCartInfo.OpenedItems : cartInfo.OpenedItems;
                cartInfo.StoredItems = (shoppingCartInfo != null) ? shoppingCartInfo.StoredItems : cartInfo.StoredItems;
                cartInfo.LeasingOptions = (shoppingCartInfo != null) ? shoppingCartInfo.LeasingOptions : cartInfo.LeasingOptions;
            }

            CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses;

            if (visitor.IsValidLogin)
            {
                CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses getDeliveryAddresses = new CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses() { DataAreaId = BaseController.DataAreaId, VisitorId = visitor.Id };

                deliveryAddresses = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses, CompanyGroup.Dto.PartnerModule.DeliveryAddresses>("Customer", "GetDeliveryAddresses", getDeliveryAddresses);
            }
            else
            {
                deliveryAddresses = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses() { Items = new List<CompanyGroup.Dto.PartnerModule.DeliveryAddress>() };
            }

            CompanyGroup.WebClient.Models.Catalogue catalogue = new CompanyGroup.WebClient.Models.Catalogue(structures, products, visitor, cartInfo.ActiveCart, cartInfo.OpenedItems, cartInfo.StoredItems, shoppingCartOpenStatus, catalogueOpenStatus, deliveryAddresses, bannerList, cartInfo.LeasingOptions);

            //aktív kosár azonosítójának mentése http cookie-ba
            if (!String.IsNullOrWhiteSpace(cartInfo.ActiveCart.Id))
            {
                visitorData.CartId = cartInfo.ActiveCart.Id;

                this.WriteCookie(visitorData);
            }

            return catalogue;
        }

        /// <summary>
        /// terméklista lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns>terméklista objektum és a látogató objektum JSON formátumban</returns>
        [HttpPost]
        [ActionName("GetProducts")]
        public CompanyGroup.WebClient.Models.Products GetProducts(CompanyGroup.Dto.ServiceRequest.GetAllProduct request) //CompanyGroup.Dto.ServiceRequest.CatalogueFilter
        {
            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            request.VisitorId = visitorData.ObjectId;   //this.ReadObjectIdFromCookie();

            request.Currency = visitorData.Currency;    // this.ReadCurrencyFromCookie();

            request.NameOrPartNumberFilter = request.NameOrPartNumberFilter ?? String.Empty;

            CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetAllProduct, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetAll", request);

            return new CompanyGroup.WebClient.Models.Products(products, visitor);
        }

        /// <summary>
        /// részletes termék adatlap
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Detail")]
        public CompanyGroup.WebClient.Models.CatalogueItem Detail()
        {
            return Details(CompanyGroup.Helpers.QueryStringParser.GetString("ProductId"));
        }

        /// <summary>
        /// részletes termék adatlap
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Details")]
        public CompanyGroup.WebClient.Models.CatalogueItem Details(string ProductId)
        {
            if (String.IsNullOrWhiteSpace(ProductId)) { ProductId = "PGI7BK"; }

            CompanyGroup.WebClient.Models.VisitorData visitorData = this.ReadCookie();

            CompanyGroup.Dto.ServiceRequest.GetItemByProductId request = new CompanyGroup.Dto.ServiceRequest.GetItemByProductId()
            {
                ProductId = ProductId,
                DataAreaId = ApiBaseController.DataAreaId,
                VisitorId = visitorData.ObjectId,
                Currency = visitorData.Currency
            };

            CompanyGroup.Dto.WebshopModule.Product product = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetItemByProductId, CompanyGroup.Dto.WebshopModule.Product>("Product", "GetItemByProductId", request);

            CompanyGroup.Dto.WebshopModule.CompatibleProducts compatibleProducts = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetItemByProductId, CompanyGroup.Dto.WebshopModule.CompatibleProducts>("Product", "GetCompatibleProducts", request);

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            CompanyGroup.Dto.ServiceRequest.GetBannerList bannerListRequest = new CompanyGroup.Dto.ServiceRequest.GetBannerList()
            {
                BscFilter = true,
                HrpFilter = true,
                Category1IdList = new List<string>() { product.FirstLevelCategory.Id },
                Category2IdList = new List<string>() { product.SecondLevelCategory.Id },
                Category3IdList = new List<string>() { product.ThirdLevelCategory.Id },
                Currency = this.ReadCurrencyFromCookie(),
                VisitorId = visitor.Id
            };

            CompanyGroup.Dto.WebshopModule.BannerList bannerList = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetBannerList, CompanyGroup.Dto.WebshopModule.BannerList>("Product", "GetBannerList", bannerListRequest);

            //kosár lekérdezések     
            bool shoppingCartOpenStatus = this.ReadShoppingCartOpenedFromCookie();

            bool catalogueOpenStatus = this.ReadCatalogueOpenedFromCookie();

            CompanyGroup.WebClient.Models.ShoppingCartInfo cartInfo = new CompanyGroup.WebClient.Models.ShoppingCartInfo();  //(visitor.IsValidLogin) ? this.GetCartInfo() : 

            if (visitor.IsValidLogin && !String.IsNullOrEmpty(visitorData.CartId))
            {
                CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo shoppingCartInfoRequest = new CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo(visitorData.CartId, visitor.Id);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetShoppingCartInfo, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "GetShoppingCartInfo", shoppingCartInfoRequest);

                cartInfo.ActiveCart = (shoppingCartInfo != null) ? shoppingCartInfo.ActiveCart : cartInfo.ActiveCart;
                cartInfo.OpenedItems = (shoppingCartInfo != null) ? shoppingCartInfo.OpenedItems : cartInfo.OpenedItems;
                cartInfo.StoredItems = (shoppingCartInfo != null) ? shoppingCartInfo.StoredItems : cartInfo.StoredItems;
                cartInfo.LeasingOptions = (shoppingCartInfo != null) ? shoppingCartInfo.LeasingOptions : cartInfo.LeasingOptions;
            }

            CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses;

            if (visitor.IsValidLogin)
            {
                CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses deliveryAddressesRequest = new CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses() { DataAreaId = WebshopApiController.DataAreaId, VisitorId = visitor.Id };

                deliveryAddresses = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses, CompanyGroup.Dto.PartnerModule.DeliveryAddresses>("CustomerService", "GetDeliveryAddresses", deliveryAddressesRequest);
            }
            else
            {
                deliveryAddresses = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses() { Items = new List<CompanyGroup.Dto.PartnerModule.DeliveryAddress>() };
            }

            //aktív kosár azonosítójának mentése http cookie-ba
            if (!String.IsNullOrWhiteSpace(cartInfo.ActiveCart.Id))
            {
                visitorData.CartId = cartInfo.ActiveCart.Id;

                this.WriteCookie(visitorData);
            }

            return new CompanyGroup.WebClient.Models.CatalogueItem(product, compatibleProducts.Items, compatibleProducts.ReverseItems, visitor, bannerList, cartInfo.ActiveCart, cartInfo.OpenedItems, cartInfo.StoredItems, shoppingCartOpenStatus, catalogueOpenStatus, deliveryAddresses, cartInfo.LeasingOptions);
        }

        /// <summary>
        /// terméknév alapján kiegészítő lista lekérdezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCompletionListAllProduct")]
        public CompanyGroup.WebClient.Models.CompletionList GetCompletionListAllProduct()
        {
            string prefix = CompanyGroup.Helpers.QueryStringParser.GetString("Prefix");

            CompanyGroup.Dto.WebshopModule.CompletionList response = this.GetJSonData<CompanyGroup.Dto.WebshopModule.CompletionList>(String.Format("Product/GetCompletionList/{0}/{1}/{2}", WebshopApiController.DataAreaId, prefix, 2));

            return new CompanyGroup.WebClient.Models.CompletionList(response);
        }

        /// <summary>
        /// termékazonosító, cikkszám, terméknév alapján kiegészítő lista lekérdezés
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("GetCompletionListBaseProduct")]
        public CompanyGroup.WebClient.Models.CompletionList GetCompletionListBaseProduct()
        {
            string prefix = CompanyGroup.Helpers.QueryStringParser.GetString("Prefix");

            CompanyGroup.Dto.WebshopModule.CompletionList response = this.GetJSonData<CompanyGroup.Dto.WebshopModule.CompletionList>(String.Format("Product/GetCompletionList/{0}/{1}/{2}", WebshopApiController.DataAreaId, prefix, 1));

            return new CompanyGroup.WebClient.Models.CompletionList(response);
        }

        [HttpGet]
        [ActionName("DownloadPriceList")]
        public System.Web.Mvc.ActionResult DownloadPriceList() //CompanyGroup.Dto.ServiceRequest.CatalogueFilter
        {
            /*
+		Request.QueryString	{ManufacturerIdList%5b%5d=A004&ActionFilter=false&BargainFilter=false&NewFilter=false&StockFilter=false&TextFilter=&HrpFilter=true&BscFilter=true&PriceFilter=0&PriceFilterRelation=0&NameOrPartNumberFilter=&Sequence=0&CurrentPageIndex=1&ItemsOnPage=30&Clear=undefined}	System.Collections.Specialized.NameValueCollection {System.Web.HttpValueCollection}
             
             */
            CompanyGroup.Dto.ServiceRequest.GetPriceList request = new CompanyGroup.Dto.ServiceRequest.GetPriceList()
            {
                ActionFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("ActionFilter", false),
                BargainFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("BargainFilter", false),
                BscFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("BscFilter", true),
                Category1IdList = ConvertDelimitedStringToList(CompanyGroup.Helpers.QueryStringParser.GetString("Category1IdList[]", "")),
                Category2IdList = ConvertDelimitedStringToList(CompanyGroup.Helpers.QueryStringParser.GetString("Category2IdList[]", "")),
                Category3IdList = ConvertDelimitedStringToList(CompanyGroup.Helpers.QueryStringParser.GetString("Category3IdList[]", "")),
                Currency = this.ReadCurrencyFromCookie(),
                HrpFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("HrpFilter", true),
                IsInNewsletterFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("IsInNewsletterFilter", false),
                ManufacturerIdList = ConvertDelimitedStringToList(CompanyGroup.Helpers.QueryStringParser.GetString("ManufacturerIdList[]", "")),
                NameOrPartNumberFilter = CompanyGroup.Helpers.QueryStringParser.GetString("NameOrPartNumberFilter", ""),
                NewFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("NewFilter", false),
                PriceFilter = CompanyGroup.Helpers.QueryStringParser.GetString("PriceFilter", ""),
                PriceFilterRelation = CompanyGroup.Helpers.QueryStringParser.GetString("PriceFilterRelation", ""),
                Sequence = CompanyGroup.Helpers.QueryStringParser.GetInt("Sequence", 0),
                StockFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("StockFilter", false),
                TextFilter = CompanyGroup.Helpers.QueryStringParser.GetString("TextFilter", ""),
                VisitorId = this.ReadObjectIdFromCookie()
            };


            request.NameOrPartNumberFilter = request.NameOrPartNumberFilter ?? String.Empty;

            CompanyGroup.Dto.WebshopModule.PriceList priceList = this.PostJSonData<CompanyGroup.Dto.ServiceRequest.GetPriceList, CompanyGroup.Dto.WebshopModule.PriceList>("Product", "GetPriceList", request);

            System.Data.DataSet ds = CreateDataSet(priceList);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            ExcelLibrary.DataSetHelper.CreateWorkbook(ms, ds);

            string fileDownloadName = "pricelist.xls";

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", fileDownloadName));

            ms.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();

            return new System.Web.Mvc.EmptyResult();
            //return File(ms, "application/vnd.ms-excel", fileDownloadName);
        }

        private System.Data.DataSet CreateDataSet(CompanyGroup.Dto.WebshopModule.PriceList from)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("CannotCancel", typeof(bool));
            dt.Columns.Add("Currency", typeof(string));
            dt.Columns.Add("DataAreaId", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("EndOfSales", typeof(bool));
            dt.Columns.Add("FirstLevelCategoryId", typeof(string));
            dt.Columns.Add("FirstLevelCategoryName", typeof(string));
            dt.Columns.Add("GarantyMode", typeof(string));
            dt.Columns.Add("GarantyTime", typeof(string));
            dt.Columns.Add("InnerStock", typeof(int));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("ManufacturerId", typeof(string));
            dt.Columns.Add("ManufacturerName", typeof(string));
            dt.Columns.Add("New", typeof(bool));
            dt.Columns.Add("OuterStock", typeof(int));
            dt.Columns.Add("PartNumber", typeof(string));
            dt.Columns.Add("Price", typeof(int));
            dt.Columns.Add("ProductId", typeof(string));
            dt.Columns.Add("PurchaseInProgress", typeof(bool));
            dt.Columns.Add("SecondLevelCategoryId", typeof(string));
            dt.Columns.Add("SecondLevelCategoryName", typeof(string));
            dt.Columns.Add("ShippingDate", typeof(DateTime));
            dt.Columns.Add("ThirdLevelCategoryId", typeof(string));
            dt.Columns.Add("ThirdLevelCategoryName", typeof(string));

            foreach (CompanyGroup.Dto.WebshopModule.PriceListItem item in from.Items)
            {
                System.Data.DataRow row = dt.NewRow();

                row["CannotCancel"] = item.CannotCancel;
                row["Currency"] = item.Currency;
                row["DataAreaId"] = item.DataAreaId;
                row["Description"] = item.Description;
                row["EndOfSales"] = item.EndOfSales;
                row["FirstLevelCategoryId"] = item.FirstLevelCategory.Id;
                row["FirstLevelCategoryName"] = item.FirstLevelCategory.Name;
                row["GarantyMode"] = item.GarantyMode;
                row["GarantyTime"] = item.GarantyTime;
                row["InnerStock"] = item.InnerStock;
                row["ItemName"] = item.ItemName;
                row["ManufacturerId"] = item.Manufacturer.Id;
                row["ManufacturerName"] = item.Manufacturer.Name;
                row["New"] = item.New;
                row["OuterStock"] = item.OuterStock;
                row["PartNumber"] = item.PartNumber;
                row["Price"] = item.Price;
                row["ProductId"] = item.ProductId;
                row["PurchaseInProgress"] = item.PurchaseInProgress;
                row["SecondLevelCategoryId"] = item.SecondLevelCategory.Id;
                row["SecondLevelCategoryName"] = item.SecondLevelCategory.Name;
                row["ShippingDate"] = item.ShippingDate;
                row["ThirdLevelCategoryId"] = item.ThirdLevelCategory.Id;
                row["ThirdLevelCategoryName"] = item.ThirdLevelCategory.Name;

                dt.Rows.Add(row);
            }

            System.Data.DataSet ds = new System.Data.DataSet();

            ds.Tables.Add(dt);

            return ds;
        }

        private List<string> ConvertDelimitedStringToList(string s)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(s))
                {
                    return new List<string>();
                }

                List<string> list = new List<string>();

                foreach (string item in s.Split(','))
                {
                    list.Add(item);
                }

                return list;
            }
            catch
            {
                return new List<string>();
            }
        }

        // GET api/webshop/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/webshop
        public void Post([FromBody]string value)
        {
        }

        // PUT api/webshop/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/webshop/5
        public void Delete(int id)
        {
        }
    }
}
