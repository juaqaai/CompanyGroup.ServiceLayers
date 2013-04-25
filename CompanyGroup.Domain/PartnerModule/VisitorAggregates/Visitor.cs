using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// Kijelentkezett, vagy bejelentkezett látogató entitás
    /// </summary>
    public class VisitorData : CompanyGroup.Domain.Core.Entity
    {
        public VisitorData()
        { 
            this.VisitorId = String.Empty;
            this.LoginIP = String.Empty;
            this.AutoLogin = false;
            this.RecId = 0;
            this.CustomerId = String.Empty;
            this.CustomerName = String.Empty;
            this.CustomerPriceGroups = new List<CustomerPriceGroup>();
            this.PersonId = String.Empty;
            this.PersonName = String.Empty;
            this.Email = String.Empty;
            this.LoggedIn = false;
            this.LoginType = LoginType.None;
            this.Permission = new PartnerModule.Permission();
            this.PartnerModel = global::PartnerModel.None;
            this.LoginDate = DateTime.MinValue;
            this.LogoutDate = DateTime.MinValue;
            this.ExpireDate = DateTime.MinValue;
            this.Status = LoginStatus.Passive;
            this.DataAreaId = String.Empty;
            this.PaymTermId = String.Empty;
            this.Currency = String.Empty;
            this.InventLocationId = String.Empty;
            this.InventLocationIdHrp = String.Empty;
            this.InventLocationIdBsc = String.Empty;
            this.LanguageId = String.Empty;
            this.DefaultPriceGroupId  = String.Empty;
            this.DefaultPriceGroupIdBsc = String.Empty;
            this.DefaultPriceGroupIdHrp = String.Empty;
            this.Representative = new Representative();
            this.Valid = false;
        }

        /// <summary>
        /// látogató azonosító
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// login ip address
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// automatikus belépés beállítása megtörtént?
        /// </summary>
        public bool AutoLogin { get; set; }

        /// <summary>
        /// ax record id
        /// </summary>
        public long RecId { get; set; }

        /// <summary>
        /// vállalat azonosítója
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// vállalat neve
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// profil kiolvasás, beállítás
        /// </summary>
        //public CompanyGroup.Domain.PartnerModule.Profile Profile { get; private set; }
        public IList<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> CustomerPriceGroups { get; set; }

        /// <summary>
        /// személy azonosítója
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// személy neve
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// bejelentkezés típusától függően vagy a kapcsolattartó email címe, vagy a cég email címe
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// bejelentkezett státusz, vagy nem 
        /// </summary>
        public bool LoggedIn { get; set; }

        /// <summary>
        /// bejelentkezés típusa (None = 0, Company = 1, Person = 2)
        /// </summary>
        public LoginType LoginType { get; set; }

        /// <summary>
        /// jogosultság beállítás
        /// </summary>
        public Permission Permission { get; set; }

        /// <summary>
        /// partnermodel (None = 0, Hrp = 1, Bsc = 2, Both = 3)
        /// </summary>
        public PartnerModel PartnerModel { get; set; }

        /// <summary>
        /// Visitor bejegyzés keletkezésének időpontja
        /// </summary>
        public DateTime LoginDate { set; get; }

        /// <summary>
        /// kijelentkezés időpontja
        /// </summary>
        public DateTime LogoutDate { set; get; }

        /// <summary>
        /// Visitor bejegyzés érvényességének időpontja
        /// </summary>
        public DateTime ExpireDate { set; get; }

        public LoginStatus Status { set; get; }

        /// <summary>
        /// vállalat
        /// </summary>
        public string DataAreaId { set; get; }

        /// <summary>
        /// vevő fizetési feltétele
        /// </summary>
        public string PaymTermId { set; get; }

        /// <summary>
        /// alapértelmezett valutanem
        /// </summary>
        public string Currency { set; get; }

        /// <summary>
        /// alapértelmezett raktár
        /// </summary>
        public string InventLocationId { set; get; }

        /// <summary>
        /// hrp alapértelmezett raktár
        /// </summary>
        public string InventLocationIdHrp { set; get; }

        /// <summary>
        /// bsc alapértelmezett raktár
        /// </summary>
        public string InventLocationIdBsc { set; get; }

        /// <summary>
        /// alapértelmezett nyelv
        /// </summary>
        public string LanguageId { set; get; }

        /// <summary>
        /// árbesorolás 
        /// </summary>
        public string DefaultPriceGroupId { set; get; }

        /// <summary>
        /// bsc árbesorolás 
        /// </summary>
        public string DefaultPriceGroupIdBsc { set; get; }

        /// <summary>
        /// hrp árbesorolás 
        /// </summary>
        public string DefaultPriceGroupIdHrp { set; get; }

        /// <summary>
        /// képviselő
        /// </summary>
        public Representative Representative { set; get; }

        /// <summary>
        /// látogatás bejegyzés akkor érvényes, ha a lejárat ideje nagyobb mint az aktuális dátum - idő értéke
        /// </summary>
        public bool Valid { set; get; }

    }

    public class Visitor : CompanyGroup.Domain.PartnerModule.VisitorData, IValidatableObject
    {
        public Visitor(CompanyGroup.Domain.PartnerModule.VisitorData visitorData)
        {
            this.SetVisitor(visitorData);

            this.PartnerModel = visitorData.PartnerModel;

            this.Permission = visitorData.Permission;

            this.PaymTermId = visitorData.PaymTermId;

            this.Representative = visitorData.Representative;

            this.LanguageId = visitorData.LanguageId;

            this.DefaultPriceGroupIdBsc = visitorData.DefaultPriceGroupIdBsc ?? "2";

            this.DefaultPriceGroupIdHrp = visitorData.DefaultPriceGroupIdHrp ?? "2";

            this.InventLocationIdBsc = visitorData.InventLocationIdBsc ?? "7000";

            this.InventLocationIdHrp = visitorData.InventLocationIdHrp ?? "KULSO";
        }

        public Visitor(List<CompanyGroup.Domain.PartnerModule.VisitorData> visitorDataList)
        {
            this.visitorDataList = visitorDataList;

            VisitorData visitorData = FindVisitorData();

            this.SetVisitor(visitorData);

            this.PartnerModel = CalculatePartnerModel();

            this.Permission = CreatePermission(GetPermissionBsc(), GetPermissionHrp());

            this.PaymTermId = CreatePaymTermId(GetPaymTermIdHrp(), GetPaymTermIdBsc());

            this.Representative = CreateRepresentative(GetRepresentativeHrp(), GetRepresentativeBsc());

            this.LanguageId = CreateLanguageId(GetLanguageIdHrp(), GetLanguageIdBsc());

            this.DefaultPriceGroupIdBsc = GetDefaultPriceGroupId(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);

            this.DefaultPriceGroupIdHrp = GetDefaultPriceGroupId(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

            this.InventLocationIdHrp = GetInventLocationId(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

            this.InventLocationIdBsc = GetInventLocationId(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);
        }

        private void SetVisitor(CompanyGroup.Domain.PartnerModule.VisitorData visitorData)
        {
            this.AutoLogin = visitorData.AutoLogin;

            this.Currency = visitorData.Currency;

            this.CustomerId = visitorData.CustomerId;

            this.CustomerName = visitorData.CustomerName;

            this.CustomerPriceGroups = visitorData.CustomerPriceGroups;

            this.DataAreaId = visitorData.DataAreaId;

            this.DefaultPriceGroupId = visitorData.DefaultPriceGroupId;

            this.Email = visitorData.Email;

            this.ExpireDate = visitorData.ExpireDate;

            this.Id = visitorData.Id;

            this.InventLocationId = visitorData.InventLocationId;

            this.LoginDate = visitorData.LoginDate;

            this.LoginIP = visitorData.LoginIP;

            this.LoginType = visitorData.LoginType;

            this.LogoutDate = visitorData.LogoutDate;

            this.PersonId = visitorData.PersonId;

            this.PersonName = visitorData.PersonName;

            this.RecId = visitorData.RecId;

            this.Status = visitorData.Status;

            this.Valid = visitorData.Valid;

            this.VisitorId = visitorData.VisitorId;        
        }

        private VisitorData FindVisitorData()
        {
            VisitorData result = null;

            if (visitorDataList.Count > 0) 
            {
                result = visitorDataList.Find(x => x.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp));

                if (result == null)
                {
                    result = visitorDataList.Find(x => x.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc));
                }
            }

            return (result == null) ? new VisitorData() : result;  
        }

        /// <summary>
        /// bejelentkezés hívás eredménye
        /// </summary>
        private List<CompanyGroup.Domain.PartnerModule.VisitorData> visitorDataList;

        #region "Permission"

        /// <summary>
        /// hrp vállalat jogosultság beállítás
        /// </summary>
        private Permission GetPermissionHrp()
        {
            return GetPermission(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);
        }

        /// <summary>
        /// bsc vállalat jogosultság beállítás
        /// </summary>
        private Permission GetPermissionBsc()
        {
            return GetPermission(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc); 
        }

        private Permission GetPermission(string dataAreaId)
        {
            VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId)) : null;

            return (visitorData == null) ? new Permission() : visitorData.Permission;        
        }

        private Permission CreatePermission(CompanyGroup.Domain.PartnerModule.Permission permissionBsc, CompanyGroup.Domain.PartnerModule.Permission permissionHrp)
        {
            return new Permission(permissionHrp.IsWebAdministrator || permissionBsc.IsWebAdministrator, 
                                  permissionHrp.InvoiceInfoEnabled || permissionBsc.InvoiceInfoEnabled, 
                                  permissionHrp.PriceListDownloadEnabled || permissionBsc.PriceListDownloadEnabled,
                                  permissionHrp.CanOrder || permissionBsc.CanOrder,
                                  permissionHrp.RecieveGoods || permissionBsc.RecieveGoods);
        }

        #endregion

        #region "PaymTermId"

        /// <summary>
        /// bsc fizetési feltételek beállítás
        /// </summary>
        private string GetPaymTermIdBsc()
        {
            return GetPaymTermId(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc); 
        }

        /// <summary>
        /// hrp fizetési feltételek beállítás
        /// </summary>
        private string GetPaymTermIdHrp()
        {
            return GetPaymTermId(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp); 
        }

        private string GetPaymTermId(string dataAreaId)
        {
            VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId)) : null;

            return (visitorData == null) ? String.Empty : visitorData.PaymTermId;
        }

        private string CreatePaymTermId(string paymTermIdHrp, string paymTermIdBsc)
        {
            if (!String.IsNullOrEmpty(paymTermIdBsc) && !String.IsNullOrEmpty(paymTermIdHrp))
            {
                return paymTermIdHrp.StartsWith("ATUT") ? paymTermIdHrp : paymTermIdBsc;
            }
            if (String.IsNullOrEmpty(paymTermIdBsc) && !String.IsNullOrEmpty(paymTermIdHrp))
            {
                return paymTermIdHrp;
            }
            if (!String.IsNullOrEmpty(paymTermIdBsc) && String.IsNullOrEmpty(paymTermIdHrp))
            {
                return paymTermIdBsc;
            }
            return CompanyGroup.Domain.Core.Constants.PaymentIdKP;
        }

        #endregion

        #region "Representative"

        /// <summary>
        /// hrp vállalat képviselő beállítás
        /// </summary>
        public Representative GetRepresentativeHrp()
        {
            return GetRepresentative(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);
        }

        /// <summary>
        /// bsc vállalat képviselő beállítás
        /// </summary>
        public Representative GetRepresentativeBsc()
        {
            return GetRepresentative(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);
        }

        private Representative GetRepresentative(string dataAreaId)
        {
            VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId)) : null;

            return (visitorData == null) ? new Representative() : visitorData.Representative;
        }

        private Representative CreateRepresentative(CompanyGroup.Domain.PartnerModule.Representative representativeBsc, CompanyGroup.Domain.PartnerModule.Representative representativeHrp)
        {
            return new Representative(!String.IsNullOrEmpty(representativeHrp.Id) ? representativeHrp.Id : representativeBsc.Id,
                                      !String.IsNullOrEmpty(representativeHrp.Name) ? representativeHrp.Name : representativeBsc.Name,
                                      CreatePhoneNumber(!String.IsNullOrEmpty(representativeHrp.Phone) ? representativeHrp.Phone : representativeBsc.Phone, !String.IsNullOrEmpty(representativeHrp.Extension) ? representativeHrp.Extension : representativeBsc.Extension), 
                                      !String.IsNullOrEmpty(representativeHrp.Mobile) ? representativeHrp.Mobile : representativeBsc.Mobile,
                                      !String.IsNullOrEmpty(representativeHrp.Extension) ? representativeHrp.Extension : representativeBsc.Extension, 
                                      !String.IsNullOrEmpty(representativeHrp.Email) ? representativeHrp.Email : representativeBsc.Email
                                      );
        }

        private static string CreatePhoneNumber(string phone, string extension)
        {
            if (String.IsNullOrEmpty(phone) && String.IsNullOrEmpty(extension))
            {
                return CompanyGroup.Domain.Core.Constants.CompanyBasePhoneNumber;
            }

            if ((String.IsNullOrEmpty(phone) || phone.Equals(CompanyGroup.Domain.Core.Constants.CompanyBasePhoneNumber)) && !String.IsNullOrEmpty(extension))
            {
                return CompanyGroup.Domain.Core.Constants.CompanyBasePhoneNumber.Replace("600", extension);
            }

            return String.IsNullOrEmpty(phone) ? CompanyGroup.Domain.Core.Constants.CompanyBasePhoneNumber : phone;
        }

        #endregion

        #region "InventLocationId"

        private string GetInventLocationId(string dataAreaId)
        {
            VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId)) : null;

            return (visitorData == null) ? String.Empty : visitorData.InventLocationId;
        }

        #endregion

        #region "LanguageId"

        /// <summary>
        /// bsc alapértelmezett nyelv
        /// </summary>
        private string GetLanguageIdBsc()
        {
            return GetLanguageId(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);        
        }

        /// <summary>
        /// hrp alapértelmezett nyelv
        /// </summary>
        private string GetLanguageIdHrp()
        {
            return GetLanguageId(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);        
        }

        private string GetLanguageId(string dataAreaId)
        {
            VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId)) : null;

            return (visitorData == null) ? String.Empty : visitorData.LanguageId;
        }

        private string CreateLanguageId(string languageIdHrp, string languageIdBsc)
        {
            return String.IsNullOrEmpty(languageIdHrp) ? languageIdBsc : languageIdHrp;
        }

        #endregion

        private string GetDefaultPriceGroupId(string dataAreaId)
        {
            VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId, StringComparison.InvariantCultureIgnoreCase)) : null;

            return (visitorData == null) ? String.Empty : visitorData.DefaultPriceGroupId;
        }

        /// <summary>
        /// bejelentkezés érvényes-e, vagy sem?
        /// </summary>
        public bool IsValidLogin
        {
            get
            {
                bool personalLoginOK = (this.LoginType == LoginType.Person) && (!String.IsNullOrWhiteSpace(this.CustomerId)) && (!String.IsNullOrWhiteSpace(this.CustomerName)) && (!String.IsNullOrWhiteSpace(this.PersonId)) && (!String.IsNullOrWhiteSpace(this.PersonName));

                bool companyLoginOK = (this.LoginType == LoginType.Company) && (!String.IsNullOrWhiteSpace(this.CustomerId)) && (!String.IsNullOrWhiteSpace(this.CustomerName));

                return (DateTime.Now.CompareTo(this.ExpireDate) < 1) && (personalLoginOK || companyLoginOK) && (this.Valid);       //(this.Status == LoginStatus.Active || this.Status == LoginStatus.Permanent);             
            }
        }

        /// <summary>
        /// bejelentkezett-e tulajdonság beállítása beállító metóduson keresztül       
        /// </summary>
        public void SetLoggedIn()
        {
            this.SetLoggedIn(this.LoginType);
        }

        /// <summary>
        /// bejelentkezett tulajdonság beállítása
        /// személyes belépés: vevőkód, vevőnév, kapcsolattartó kód, kapcsolattartó név nem lehet üres
        /// céges belépés:     vevőkód, vevőnév nem lehet üres  
        /// </summary>
        /// <param name="loginType"></param>
        public void SetLoggedIn(LoginType loginType)
        {
            bool personalLoginOK = (loginType == LoginType.Person) && (!String.IsNullOrWhiteSpace(this.CustomerId)) && (!String.IsNullOrWhiteSpace(this.CustomerName)) && (!String.IsNullOrWhiteSpace(this.PersonId)) && (!String.IsNullOrWhiteSpace(this.PersonName));

            bool companyLoginOK = (loginType == LoginType.Company) && (!String.IsNullOrWhiteSpace(this.CustomerId)) && (!String.IsNullOrWhiteSpace(this.CustomerName));

            this.LoggedIn = DateTime.Now.CompareTo(this.ExpireDate) < 1 && (loginType > 0) && (personalLoginOK || companyLoginOK) && (this.Status == LoginStatus.Active || this.Status == LoginStatus.Permanent);
        }

        /// <summary>
        /// szerepkörök beállítása a jogosultságkezeléshez
        /// </summary>
        public List<string> Roles
        {
            get
            {
                List<string> roles = new List<string>();

                if (this.Permission.IsWebAdministrator)
                {
                    roles.Add("WebAdministrator");
                }
                if (this.Permission.InvoiceInfoEnabled)
                {
                    roles.Add("InvoiceInfoReader");
                }
                if (this.Permission.PriceListDownloadEnabled)
                {
                    roles.Add("PriceListReader");
                }
                if (this.Permission.CanOrder)
                {
                    roles.Add("CanOrder");
                }
                return roles;
            }
            set { }
        }

        /// <summary>
        /// jogosultság beállítása
        /// </summary>
        public void SetPermission(Permission permission)
        {

            if (permission == null)
            {
                throw new ArgumentNullException("permission");
            }

            this.Permission = permission;
        }

        /// <summary>
        /// árkalkuláció
        /// </summary>
        /// <param name="price1">termék ár1</param>
        /// <param name="price2">termék ár2</param>
        /// <param name="price3">termék ár3</param>
        /// <param name="price4">termék ár4</param>
        /// <param name="price5">termék ár5</param>
        /// <param name="manufacturerId">termék gyártó</param>
        /// <param name="category1Id">termék jelleg1</param>
        /// <param name="category2Id">termék jelleg2</param>
        /// <param name="category3Id">termék jelleg3</param>
        /// <param name="dataAreaId">termék vállalatkódja</param>
        /// <returns></returns>
        public decimal CalculateCustomerPrice2(decimal price1, decimal price2, decimal price3, decimal price4, decimal price5,
                                              string manufacturerId, string category1Id, string category2Id, string category3Id, string dataAreaId)
        {
            //szűrés vállalatkódra, azaz csak azok az árcsoprotok kerülnek az eredményhalmazba, melyek vállalatkódja egyezik a termék vállalatkódjával
            IEnumerable<CustomerPriceGroup> priceGroups = this.CustomerPriceGroups.Where(x => (x.DataAreaId.Equals(dataAreaId, StringComparison.OrdinalIgnoreCase)));

            //szűrt lista másolása
            List<CustomerPriceGroup> priceGroupList = new List<CustomerPriceGroup>();

            //másolás érték szerint
            foreach (CustomerPriceGroup p in priceGroups)
            {
                priceGroupList.Add(new CustomerPriceGroup(p.LineId, p.VisitorKey, p.PriceGroupId, p.ManufacturerId, p.Category1Id, p.Category2Id, p.Category3Id, p.Order, p.DataAreaId));
            }

            //listaelemek üres gyártó és jelleg1 - jelleg2 - jelleg3 elemek feltöltése a termék gyártó, jelleg1 - jelleg2 - jelleg3 adataival, ha azok nem üresek
            priceGroupList.ForEach(x => {
                if ((String.IsNullOrEmpty(x.Category1Id) || x.Category1Id.Equals("____", StringComparison.OrdinalIgnoreCase))) { x.Category1Id = category1Id; }             // && !String.IsNullOrEmpty(category1Id)
                if ((String.IsNullOrEmpty(x.Category2Id) || x.Category2Id.Equals("____", StringComparison.OrdinalIgnoreCase))) { x.Category2Id = category2Id; }             // && !String.IsNullOrEmpty(category2Id)
                if ((String.IsNullOrEmpty(x.Category3Id) || x.Category3Id.Equals("____", StringComparison.OrdinalIgnoreCase))) { x.Category3Id = category3Id; }             // && !String.IsNullOrEmpty(category3Id)
                if ((String.IsNullOrEmpty(x.ManufacturerId) || x.ManufacturerId.Equals("____", StringComparison.OrdinalIgnoreCase))) { x.ManufacturerId = manufacturerId; } // && !String.IsNullOrEmpty(manufacturerId)
            });

            // || x.ManufacturerId.Equals("____", StringComparison.OrdinalIgnoreCase)
            IEnumerable<CustomerPriceGroup> searchResults = priceGroupList.Where(x => (x.ManufacturerId.Equals(manufacturerId, StringComparison.OrdinalIgnoreCase)) &&
                                                                                      (x.Category1Id.Equals(category1Id, StringComparison.OrdinalIgnoreCase)) &&
                                                                                      (x.Category2Id.Equals(category2Id, StringComparison.OrdinalIgnoreCase)) &&
                                                                                      (x.Category3Id.Equals(category3Id, StringComparison.OrdinalIgnoreCase)));

            CustomerPriceGroup priceGroup;

            if (searchResults.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = searchResults.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //alapártelmezett árat kell visszaadni
            string defaultPriceGroup = (dataAreaId.Equals(Core.Constants.DataAreaIdHrp)) ? this.DefaultPriceGroupIdHrp : this.DefaultPriceGroupIdBsc;

            return LookupPrice(price1, price2, price3, price4, price5, (String.IsNullOrEmpty(DefaultPriceGroupId) ? "2" : defaultPriceGroup));
        }

        /// <summary>
        /// vevő saját árának kalkulációja
        /// </summary>
        /// <param name="price1"></param>
        /// <param name="price2"></param>
        /// <param name="price3"></param>
        /// <param name="price4"></param>
        /// <param name="price5"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="category1Id"></param>
        /// <param name="category2Id"></param>
        /// <param name="category3Id"></param>
        /// <param name="dataAreaId"></param>
        /// <![CDATA[
        ///     Id	VisitorId	ManufacturerId	Category1Id	Category2Id	Category3Id	PriceGroupId	Order   DataAreaId
        ///    2370	247					                                                        3	150     bsc
        ///    2371	247					                                                        2	170     hrp
        ///    2372	247	        A010				                                            4	150     hrp
        ///    2373	247	        A017				                                            4	150     hrp
        ///    2374	247	        A029				                                            4	150     hrp
        ///    2375	247	        A036				                                            5	140     hrp
        ///    2376	247	        A040				                                            4	150     hrp
        ///    2377	247	        A044				                                            5	140     hrp
        ///    2378	247	        A052	           B007			                                5	140     hrp
        ///    2379	247	        A057				                                            4	150     hrp
        ///    2380	247	        A058				                                            3	160     hrp
        ///    2381	247	        A075				                                            4	150     hrp
        ///    2382	247	        A083	____	____	____	                                3	150     hrp
        ///    2383	247	        A090				                                            4	150     hrp
        ///    2384	247	        A093				                                            4	150     hrp
        ///    2385	247	        A098				                                            4	150     hrp
        ///    2386	247	        A102	____	____	____	                                3	150     hrp
        ///    2387	247	        A122				                                            5	140     hrp
        ///    2388	247	        A125				                                            3	160     hrp
        ///    2389	247	        A133				                                            4	150     hrp
        ///    2390	247	        A142				                                            4	150     hrp
        /// ]]>
        /// <returns></returns>
        public decimal CalculateCustomerPrice(decimal price1, decimal price2, decimal price3, decimal price4, decimal price5,
                                              string manufacturerId, string category1Id, string category2Id, string category3Id, string dataAreaId)
        {
            CustomerPriceGroup priceGroup;

            //vizsgálat teljes egyezésre, gyártó - jelleg1 - jelleg2 - jelleg3, vagy üres árbesorolás gyártó és termék gyártó - üres árbesorolás jelleg1 és termék jelleg1 - üres árbesorolás jelleg2 és termék jelleg2 - üres árbesorolás jelleg3 és termék jelleg3
            IEnumerable<CustomerPriceGroup> priceGroups = this.CustomerPriceGroups.Where(x =>
                                                                                        ((x.ManufacturerId == manufacturerId) || (String.IsNullOrEmpty(x.ManufacturerId) && String.IsNullOrEmpty(manufacturerId))) &&
                                                                                        ((x.Category1Id == category1Id) || (String.IsNullOrEmpty(x.Category1Id) && String.IsNullOrEmpty(category1Id))) &&
                                                                                        ((x.Category2Id == category2Id) || (String.IsNullOrEmpty(x.Category2Id) && String.IsNullOrEmpty(category2Id))) &&
                                                                                        ((x.Category3Id == category3Id) || (String.IsNullOrEmpty(x.Category3Id) && String.IsNullOrEmpty(category3Id))) &&
                                                                                        (x.DataAreaId.ToLower() == dataAreaId.ToLower()));
            //ha van találat teljes egyezésre, akkor a legalacsonyabb sorszámút kell kiolvasni
            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //vizsgálat gyártó - jelleg1 - jelleg2 egyezésre, vagy üres árbesorolás gyártó és termék  gyártó - üres árbesorolás jelleg1 és termék jelleg1 - üres árbesorolás jelleg2 és termék jelleg2
            priceGroups = this.CustomerPriceGroups.Where(x =>
                                                        ((x.ManufacturerId == manufacturerId) || (String.IsNullOrEmpty(x.ManufacturerId) && String.IsNullOrEmpty(manufacturerId))) &&
                                                        ((x.Category1Id == category1Id) || (String.IsNullOrEmpty(x.Category1Id) && String.IsNullOrEmpty(category1Id))) &&
                                                        ((x.Category2Id == category2Id) || (String.IsNullOrEmpty(x.Category2Id) && String.IsNullOrEmpty(category2Id))) &&
                                                        (x.DataAreaId.ToLower() == dataAreaId.ToLower()));
            //ha van találat teljes egyezésre, akkor a legalacsonyabb sorszámút kell kiolvasni
            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //vizsgálat gyártó - jelleg1 - egyezésre, vagy üres árbesorolás gyártó és termék gyártó - üres árbesorolás jelleg1 és termék jelleg1
            priceGroups = this.CustomerPriceGroups.Where(x =>
                                                        ((x.ManufacturerId == manufacturerId) || (String.IsNullOrEmpty(x.ManufacturerId) && String.IsNullOrEmpty(manufacturerId))) &&
                                                        ((x.Category1Id == category1Id) || (String.IsNullOrEmpty(x.Category1Id) && String.IsNullOrEmpty(category1Id))) &&
                                                        (x.DataAreaId.ToLower() == dataAreaId.ToLower()));
            //ha van találat teljes egyezésre, akkor a legalacsonyabb sorszámút kell kiolvasni
            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //vizsgálat gyártó - egyezésre, vagy üres árbesorolás gyártó és termék gyártó
            priceGroups = this.CustomerPriceGroups.Where(x => ((x.ManufacturerId == manufacturerId) || (String.IsNullOrEmpty(x.ManufacturerId) && String.IsNullOrEmpty(manufacturerId))) && (x.DataAreaId.ToLower() == dataAreaId.ToLower()));

            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }


            //*****vizsgálat üres gyártó - nem üres jelleg1, jelleg2 - egyezésre
            priceGroups = this.CustomerPriceGroups.Where(x =>
                                                        (String.IsNullOrEmpty(x.ManufacturerId)) &&
                                                        ((x.Category1Id == category1Id) && !String.IsNullOrEmpty(x.Category1Id)) &&
                                                        ((x.Category2Id == category2Id) && !String.IsNullOrEmpty(x.Category2Id)) &&
                                                        (x.DataAreaId.ToLower() == dataAreaId.ToLower()));
            //ha van találat teljes egyezésre, akkor a legalacsonyabb sorszámút kell kiolvasni
            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //*****vizsgálat üres gyártó - nem üres jelleg1 - egyezésre
            priceGroups = this.CustomerPriceGroups.Where(x =>
                                                        (String.IsNullOrEmpty(x.ManufacturerId)) &&
                                                        ((x.Category1Id == category1Id) && !String.IsNullOrEmpty(x.Category1Id)) &&
                                                        (x.DataAreaId.ToLower() == dataAreaId.ToLower()));
            //ha van találat teljes egyezésre, akkor a legalacsonyabb sorszámút kell kiolvasni
            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //vizsgálat üres gyártó, jelleg1, jelleg2, jelleg3 -ra
            priceGroups = this.CustomerPriceGroups.Where(x => (x.ManufacturerId == "" && x.Category1Id == "" && x.Category2Id == "" && x.Category3Id == "" && x.DataAreaId.ToLower() == dataAreaId.ToLower()));

            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //alapártelmezett 2-es árat kell visszaadni
            return LookupPrice(price1, price2, price3, price4, price5, "2");
        }

        /// <summary>
        /// ár kikeresése árcsoport alapján
        /// </summary>
        /// <param name="price1"></param>
        /// <param name="price2"></param>
        /// <param name="price3"></param>
        /// <param name="price4"></param>
        /// <param name="price5"></param>
        /// <param name="priceGroup"></param>
        /// <returns></returns>
        private decimal LookupPrice(decimal price1, decimal price2, decimal price3, decimal price4, decimal price5, string priceGroup)
        {
            if (priceGroup.Equals("1")) { return price1; }
            if (priceGroup.Equals("2")) { return price2; }
            if (priceGroup.Equals("3")) { return price3; }
            if (priceGroup.Equals("4")) { return price4; }
            if (priceGroup.Equals("5")) { return price5; }

            return price2;
        }

        /// <summary>
        /// bool canRead = ((PermissionTypes.Read & admin.Permissions) == PermissionTypes.Read);
        /// </summary>
        /// <returns></returns>
        public PartnerModel CalculatePartnerModel()
        {
            PartnerModel result = global::PartnerModel.None;

            if (this.visitorDataList.Exists(x => (x.PartnerModel & global::PartnerModel.Bsc) == global::PartnerModel.Bsc))
            {
                result = global::PartnerModel.Bsc;
            }
            if (this.visitorDataList.Exists(x => (x.PartnerModel & global::PartnerModel.Hrp) == global::PartnerModel.Hrp))
            {
                result = global::PartnerModel.Hrp;
            }
            if (this.visitorDataList.Exists(x => (x.PartnerModel & global::PartnerModel.Hrp) == global::PartnerModel.Hrp && (x.PartnerModel & global::PartnerModel.Bsc) == global::PartnerModel.Bsc))
            {
                result = global::PartnerModel.Both;
            }

            return result;
        }

        /// <summary>
        /// hrp vállalatban jogosultság létezik-e?
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorizedInHrp()
        {
            return this.PartnerModel.Equals(PartnerModel.Hrp) || this.PartnerModel.Equals(PartnerModel.Both);
        }

        /// <summary>
        /// bsc vállalatban jogosultság létezik-e?
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorizedInBsc()
        {
            return this.PartnerModel.Equals(PartnerModel.Bsc) || this.PartnerModel.Equals(PartnerModel.Both);
        }

        /// <summary>
        /// visszaadja azt a vállalatkód listát, melyben a bejelentkezett felhasználó megtalálható
        /// </summary>
        /// <returns></returns>
        public List<string> AuthorizedDataAreaList()
        {
            List<string> dataAreaList = new List<string>();

            if (this.PartnerModel.Equals(PartnerModel.Both))
            {
                dataAreaList.Add(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                dataAreaList.Add(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);
            }
            else
            {
                if (this.PartnerModel.Equals(PartnerModel.Hrp))
                {
                    dataAreaList.Add(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);
                }
                if (this.PartnerModel.Equals(PartnerModel.Bsc))
                {
                    dataAreaList.Add(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);
                }
            }
            return dataAreaList;
        }

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (this.IsTransient())
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "ObjectId" }));
            }

            return validationResults;
        }

        public bool EqualsVisitor(Visitor obj)
        {
            if (obj == null || !(obj is Visitor))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return (obj.CustomerId == this.CustomerId) && (obj.PersonId == this.PersonId);
            }
        }
    }
}
