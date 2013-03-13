using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompanyGroup.WebClient.Controllers
{
    public class WebshopController : BaseController
    {

        /// <summary>
        /// Webshop view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string searchText = CompanyGroup.Helpers.QueryStringParser.GetString("q");

            ViewBag.Message = "Webshop view";

            //látogatóhoz nyilvántartott adatok kiolvasása sütiből
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            //bejelentkezett felhasználó adatainak kiolvasása
            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            //struktúrák lekérdezése
            CompanyGroup.Dto.WebshopModule.GetAllStructureRequest structuresRequest = new CompanyGroup.Dto.WebshopModule.GetAllStructureRequest()
            {
                DiscountFilter = false,
                SecondhandFilter = false,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                HrpFilter = true,
                BscFilter = true,
                IsInNewsletterFilter = false,
                ManufacturerIdList = new List<string>(),
                NewFilter = false,
                StockFilter = false,
                TextFilter = searchText,
                PriceFilter = "0",
                PriceFilterRelation = "0"
            };

            CompanyGroup.Dto.WebshopModule.Structures structures = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetAllStructureRequest, CompanyGroup.Dto.WebshopModule.Structures>("Structure", "GetStructures", structuresRequest);

            //katalógus lekérdezése
            CompanyGroup.Dto.WebshopModule.GetAllProductRequest productsRequest = new CompanyGroup.Dto.WebshopModule.GetAllProductRequest()
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
                TextFilter = searchText,
                PriceFilter = "0",
                PriceFilterRelation = "0",
                VisitorId = visitor.Id
            };

            CompanyGroup.Dto.WebshopModule.Products products = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetAllProductRequest, CompanyGroup.Dto.WebshopModule.Products>("Product", "GetProducts", productsRequest);

            //banner lista lekérdezése
            CompanyGroup.Dto.WebshopModule.GetBannerListRequest bannerListRequest = new CompanyGroup.Dto.WebshopModule.GetBannerListRequest()
            {
                BscFilter = true,
                HrpFilter = true,
                Category1IdList = new List<string>(),
                Category2IdList = new List<string>(),
                Category3IdList = new List<string>(),
                Currency = visitorData.Currency,
                VisitorId = visitor.Id
            };

            CompanyGroup.Dto.WebshopModule.BannerList bannerList = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetBannerListRequest, CompanyGroup.Dto.WebshopModule.BannerList>("Product", "GetBannerList", bannerListRequest);

            //kosár lekérdezések     
            List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart> storedShoppingCart;

            List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart> openedShoppingCart;

            CompanyGroup.Dto.WebshopModule.ShoppingCart activeCart;

            CompanyGroup.Dto.WebshopModule.LeasingOptions leasingOptions;

            CompanyGroup.Dto.PartnerModule.DeliveryAddresses deliveryAddresses;

            if (visitor.IsValidLogin)
            {
                CompanyGroup.Dto.WebshopModule.GetActiveCartRequest activeCartRequest = new CompanyGroup.Dto.WebshopModule.GetActiveCartRequest(visitorData.Language, visitor.Id);

                CompanyGroup.Dto.WebshopModule.ShoppingCartInfo shoppingCartInfo = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetActiveCartRequest, CompanyGroup.Dto.WebshopModule.ShoppingCartInfo>("ShoppingCart", "GetActiveCart", activeCartRequest);

                activeCart = shoppingCartInfo.ActiveCart;

                openedShoppingCart = shoppingCartInfo.OpenedItems;

                storedShoppingCart = shoppingCartInfo.StoredItems;

                leasingOptions = shoppingCartInfo.LeasingOptions;
            }
            else
            {
                activeCart = new CompanyGroup.Dto.WebshopModule.ShoppingCart();

                openedShoppingCart = new List<CompanyGroup.Dto.WebshopModule.OpenedShoppingCart>();

                storedShoppingCart = new List<CompanyGroup.Dto.WebshopModule.StoredShoppingCart>();

                leasingOptions = new CompanyGroup.Dto.WebshopModule.LeasingOptions();            
            }

            if (visitor.IsValidLogin)
            {
                CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest getDeliveryAddresses = new CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest(BaseController.DataAreaId, visitor.Id);

                deliveryAddresses = this.PostJSonData<CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest, CompanyGroup.Dto.PartnerModule.DeliveryAddresses>("Customer", "GetDeliveryAddresses", getDeliveryAddresses);
            }
            else
            {
                deliveryAddresses = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses();
            }

            CompanyGroup.WebClient.Models.Catalogue model = new CompanyGroup.WebClient.Models.Catalogue(structures, products, visitor, activeCart, openedShoppingCart, storedShoppingCart, visitorData.IsShoppingCartOpened, visitorData.IsCatalogueOpened, productsRequest.Sequence, deliveryAddresses, bannerList, leasingOptions);

            //aktív kosár azonosítójának mentése http cookie-ba
            if (activeCart.Id > 0)
            {
                visitorData.CartId = activeCart.Id;

                string json = CompanyGroup.Helpers.JsonConverter.ToJSON<CompanyGroup.WebClient.Models.VisitorData>(visitorData);

                CompanyGroup.Helpers.CookieHelper.WriteCookie(this.Response, CookieName, json);
            }

            return View(model);
        }

        /// <summary>
        /// Webshop details view kezdőérték beállításokkal
        /// </summary>
        /// <returns></returns>
        public ActionResult Details(string productId)
        {
            ViewBag.Message = "Details webshop view";

            return View();
        }

        /// <summary>
        /// árlista letöltése
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("DownloadPriceList")]
        public FileStreamResult DownloadPriceList() //System.Net.Http.HttpResponseMessage System.Web.Mvc.ActionResult  CompanyGroup.Dto.ServiceRequest.GetPriceList request
        {
            /*
+		Request.QueryString	{ManufacturerIdList%5b%5d=A004&ActionFilter=false&BargainFilter=false&NewFilter=false&StockFilter=false&TextFilter=&HrpFilter=true&BscFilter=true&PriceFilter=0&PriceFilterRelation=0&NameOrPartNumberFilter=&Sequence=0&CurrentPageIndex=1&ItemsOnPage=30&Clear=undefined}	System.Collections.Specialized.NameValueCollection {System.Web.HttpValueCollection}
             
             */
            CompanyGroup.WebClient.Models.VisitorData visitorData = CompanyGroup.Helpers.CookieHelper.ReadCookie<CompanyGroup.WebClient.Models.VisitorData>(System.Web.HttpContext.Current.Request, CookieName);

            if (visitorData == null) { visitorData = new CompanyGroup.WebClient.Models.VisitorData(); }

            CompanyGroup.WebClient.Models.Visitor visitor = this.GetVisitor(visitorData);

            CompanyGroup.Dto.WebshopModule.GetPriceListRequest request = new CompanyGroup.Dto.WebshopModule.GetPriceListRequest()
            {
                ActionFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("ActionFilter", false),
                BargainFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("BargainFilter", false),
                BscFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("BscFilter", true),
                Category1IdList = ConvertDelimitedStringToList(CompanyGroup.Helpers.QueryStringParser.GetString("Category1IdList[]", "")),
                Category2IdList = ConvertDelimitedStringToList(CompanyGroup.Helpers.QueryStringParser.GetString("Category2IdList[]", "")),
                Category3IdList = ConvertDelimitedStringToList(CompanyGroup.Helpers.QueryStringParser.GetString("Category3IdList[]", "")),
                Currency = visitorData.Currency,
                HrpFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("HrpFilter", true),
                IsInNewsletterFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("IsInNewsletterFilter", false),
                ManufacturerIdList = ConvertDelimitedStringToList(CompanyGroup.Helpers.QueryStringParser.GetString("ManufacturerIdList[]", "")),
                NewFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("NewFilter", false),
                PriceFilter = CompanyGroup.Helpers.QueryStringParser.GetString("PriceFilter", ""),
                PriceFilterRelation = CompanyGroup.Helpers.QueryStringParser.GetString("PriceFilterRelation", ""),
                Sequence = CompanyGroup.Helpers.QueryStringParser.GetInt("Sequence", 0),
                StockFilter = CompanyGroup.Helpers.QueryStringParser.GetBool("StockFilter", false),
                TextFilter = CompanyGroup.Helpers.QueryStringParser.GetString("TextFilter", ""),
                VisitorId = visitorData.VisitorId
            };

            request.VisitorId = visitorData.VisitorId;

            CompanyGroup.Dto.WebshopModule.PriceList priceList = this.PostJSonData<CompanyGroup.Dto.WebshopModule.GetPriceListRequest, CompanyGroup.Dto.WebshopModule.PriceList>("Product", "GetPriceList", request);

            //System.Data.DataSet ds = CreateDataSet(priceList);

            byte[] arr = GenerateExcelPriceList(priceList);

            System.IO.MemoryStream ms = new System.IO.MemoryStream(arr);

            //ExcelLibrary.DataSetHelper.CreateWorkbook(ms, ds);

            string fileDownloadName = "pricelist.xlsx";

            System.Net.Http.HttpResponseMessage result = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);

            result.Content = new System.Net.Http.StreamContent(ms);

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.ms-excel");

            //result.Content.Headers.Add("content-disposition", String.Format("attachment;filename={0}", fileDownloadName));
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");

            result.Content.Headers.ContentDisposition.FileName = fileDownloadName;

            //return result;

            //Response.Clear();
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", fileDownloadName));

            //ms.WriteTo(Response.OutputStream);
            //Response.Flush();
            //Response.End();

            //return new System.Web.Mvc.EmptyResult();
            return File(ms, "application/octet-stream", fileDownloadName);
        }

        #region "Epplus"

        private static byte[] GenerateExcelPriceList(CompanyGroup.Dto.WebshopModule.PriceList priceList)
        {
            using (OfficeOpenXml.ExcelPackage p = new OfficeOpenXml.ExcelPackage())
            {
                //munkafüzet tulajdonságainak beállítása
                SetWorkbookProperties(p);
                
                //alapértelmezett munkalap hozzáadása
                OfficeOpenXml.ExcelWorksheet ws = CreateSheet(p, "Terméklista");

                System.Data.DataTable dt = CreateDataTable(priceList); 

                //Merging cells and create a center heading for out table
                ws.Cells[1, 1].Value = "Terméklista excel export";
                ws.Cells[1, 1, 1, dt.Columns.Count].Merge = true;
                ws.Cells[1, 1, 1, dt.Columns.Count].Style.Font.Bold = true;
                ws.Cells[1, 1, 1, dt.Columns.Count].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                int rowIndex = 2;

                CreateHeader(ws, ref rowIndex, dt);
                CreateData(ws, ref rowIndex, dt);
                CreateFooter(ws, ref rowIndex, dt);

                //AddComment(ws, 5, 10, "Zeeshan Umar's Comments", "Zeeshan Umar");

                //string path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Application.StartupPath)), "Zeeshan Umar.jpg");
                //AddImage(ws, 10, 0, path);

                //AddCustomShape(ws, 10, 7, eShapeStyle.Ellipse, "Text inside Ellipse.");

                //Generate A File with Random name
                //Byte[] bin = 
                
                return p.GetAsByteArray();
                
                //string file = Guid.NewGuid().ToString() + ".xlsx";
                //System.IO.File.WriteAllBytes(file, bin);

                //These lines will open it in Excel
                //ProcessStartInfo pi = new ProcessStartInfo(file);
                //Process.Start(pi);
            }
        }

        /// <summary>
        /// első munkalap elkészítése
        /// </summary>
        /// <param name="p"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private static OfficeOpenXml.ExcelWorksheet CreateSheet(OfficeOpenXml.ExcelPackage p, string sheetName)
        {
            p.Workbook.Worksheets.Add(sheetName);
            OfficeOpenXml.ExcelWorksheet ws = p.Workbook.Worksheets[1];
            ws.Name = sheetName; 
            ws.Cells.Style.Font.Size = 11; 
            ws.Cells.Style.Font.Name = "Calibri"; 
            return ws;
        }

        /// <summary>
        /// excel munkafüzet jellemzők beállítása
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        private static void SetWorkbookProperties(OfficeOpenXml.ExcelPackage p)
        {
            p.Workbook.Properties.Author = "HRP Hungary Kft.";
            p.Workbook.Properties.Title = "HRP - BSC árlista";
        }

        private static void CreateHeader(OfficeOpenXml.ExcelWorksheet ws, ref int rowIndex, System.Data.DataTable dt)
        {
            int colIndex = 1;
            foreach (System.Data.DataColumn dc in dt.Columns) //Creating Headings
            {
                var cell = ws.Cells[rowIndex, colIndex];

                //Setting the background color of header cells to Gray
                var fill = cell.Style.Fill;
                fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);

                //Setting Top/left,right/bottom borders.
                var border = cell.Style.Border;
                border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                //Setting Value in cell
                cell.Value = dc.ColumnName;

                colIndex++;
            }
        }

        private static void CreateData(OfficeOpenXml.ExcelWorksheet ws, ref int rowIndex, System.Data.DataTable dt)
        {
            int colIndex = 0;

            foreach (System.Data.DataRow dr in dt.Rows) 
            {
                colIndex = 1;
                rowIndex++;

                foreach (System.Data.DataColumn dc in dt.Columns)
                {
                    var cell = ws.Cells[rowIndex, colIndex];

                    //cella értékének beállítása
                    cell.Value = Convert.ToString(dr[dc.ColumnName]);

                    //cella border beállítása
                    var border = cell.Style.Border;
                    border.Left.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    colIndex++;
                }
            }
        }

        private static void CreateFooter(OfficeOpenXml.ExcelWorksheet ws, ref int rowIndex, System.Data.DataTable dt)
        {
            int colIndex = 0;
            foreach (System.Data.DataColumn dc in dt.Columns) //Creating Formula in footers
            {
                colIndex++;
                var cell = ws.Cells[rowIndex, colIndex];

                //Setting Sum Formula
                cell.Formula = "Sum(" + ws.Cells[3, colIndex].Address + ":" + ws.Cells[rowIndex - 1, colIndex].Address + ")";

                //Setting Background fill color to Gray
                cell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
            }
        }

        /// <summary>
        /// árlista datatable-ba történő konvertálása
        /// </summary>
        /// <param name="priceList"></param>
        /// <returns></returns>
        private static System.Data.DataTable CreateDataTable(CompanyGroup.Dto.WebshopModule.PriceList priceList)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add("Termékazonosító", typeof(string));
            dt.Columns.Add("Termék neve", typeof(string));
            dt.Columns.Add("Készlet", typeof(int));
            dt.Columns.Add("Nettó ár", typeof(int));
            dt.Columns.Add("Pénznem", typeof(string));
            dt.Columns.Add("Leírás", typeof(string));
            dt.Columns.Add("Disztibútor", typeof(string));
            dt.Columns.Add("Gyártó", typeof(string));
            dt.Columns.Add("Jelleg 1", typeof(string));
            dt.Columns.Add("Jelleg 2", typeof(string));
            dt.Columns.Add("Jelleg 3", typeof(string));
            dt.Columns.Add("Gyártói cikkszám", typeof(string));
            dt.Columns.Add("Garancia módja", typeof(string));
            dt.Columns.Add("Garancia ideje", typeof(string));
            dt.Columns.Add("Újdonság", typeof(bool));
            dt.Columns.Add("Szállítási infó", typeof(DateTime));

            foreach (CompanyGroup.Dto.WebshopModule.PriceListItem item in priceList.Items)
            {
                System.Data.DataRow row = dt.NewRow();

                row["Termékazonosító"] = item.ProductId;
                row["Termék neve"] = item.ItemName;
                row["Készlet"] = item.Stock;
                row["Nettó ár"] = item.Price;
                row["Pénznem"] = item.Currency;
                row["Leírás"] = item.Description;
                row["Disztibútor"] = item.DataAreaId;
                row["Gyártó"] = item.Manufacturer.Name;
                row["Jelleg 1"] = item.FirstLevelCategory.Name;
                row["Jelleg 2"] = item.SecondLevelCategory.Name;
                row["Jelleg 3"] = item.ThirdLevelCategory.Name;
                row["Gyártói cikkszám"] = item.PartNumber;
                row["Garancia módja"] = item.GarantyMode;
                row["Garancia ideje"] = item.GarantyTime;
                row["Újdonság"] = item.New;
                row["Szállítási infó"] = item.PurchaseInProgress ? item.ShippingDate : DateTime.MinValue;

                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion

        private System.Data.DataSet CreateDataSet(CompanyGroup.Dto.WebshopModule.PriceList from)
        {
            System.Data.DataTable dt = CreateDataTable(from);

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
    }
}
