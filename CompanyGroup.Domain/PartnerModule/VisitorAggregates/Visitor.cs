﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// </summary>
    /// <example>
    /// Id VisitorId LoginIP RecId CustomerId CustomerName  
    /// PersonId    PersonName   Email                                                                                                
    /// IsWebAdministrator InvoiceInfoEnabled PriceListDownloadEnabled CanOrder RecieveGoods PaymTermIdBsc PaymTermIdHrp Currency   LanguageId DefaultPriceGroupIdHrp DefaultPriceGroupIdBsc InventLocationIdHrp InventLocationIdBsc 
    /// RepresentativeId RepresentativeName RepresentativePhone RepresentativeMobile RepresentativeExtension RepresentativeEmail
    /// LoginType RightHrp RightBsc ContractHrp ContractBsc CartId RegistrationId  IsCatalogueOpened IsShoppingCartOpened AutoLogin LoginDate LogoutDate ExpireDate
    /// Valid
    /// </example>
    public class Visitor : CompanyGroup.Domain.Core.Entity, System.ComponentModel.DataAnnotations.IValidatableObject
    {

        /// <summary>
        /// látogató azonosító
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// login ip address
        /// </summary>
        public string LoginIP { get; set; }

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
        /// vevőhöz tartozó árbesorolások 
        /// </summary>
        public List<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> CustomerPriceGroups { get; set; }

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
        /// van-e jogosultság a hrp-ben?
        /// </summary>
        public bool RightHrp { get; set; }

        /// <summary>
        /// van-e jogosultság a bsc-ben?
        /// </summary>
        public bool RightBsc { get; set; }

        /// <summary>
        /// van-e szerződés a hrp-ben?
        /// </summary>
        public bool ContractHrp { get; set; }

        /// <summary>
        /// van-e szerződés a bsc-ben?
        /// </summary>
        public bool ContractBsc { get; set; }

        /// <summary>
        /// aktív kosár azonosízó
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// regisztráció azonosítója?
        /// </summary>
        public string RegistrationId { get; set; }

        /// <summary>
        /// nyitva van-e a katalógus?
        /// </summary>
        public bool IsCatalogueOpened { get; set; }

        /// <summary>
        /// nyitva van-e a kosár lista?
        /// </summary>
        public bool IsShoppingCartOpened { get; set; }

        /// <summary>
        /// automatikus belépés beállítása megtörtént?
        /// </summary>
        public bool AutoLogin { get; set; }

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
        /// vevő fizetési feltétele hrp vállalatban
        /// </summary>
        public string PaymTermIdHrp { set; get; }

        /// <summary>
        /// vevő fizetési feltétele hrp vállalatban
        /// </summary>
        public string PaymTermIdBsc { set; get; }

        /// <summary>
        /// alapértelmezett valutanem
        /// </summary>
        public string Currency { set; get; }

        /// <summary>
        /// alapértelmezett raktár hrp vállalatban
        /// </summary>
        public string InventLocationIdHrp { set; get; }

        /// <summary>
        /// alapértelmezett raktár bsc vállalatban
        /// </summary>
        public string InventLocationIdBsc { set; get; }

        /// <summary>
        /// alapértelmezett nyelv
        /// </summary>
        public string LanguageId { set; get; }

        /// <summary>
        /// árbesorolás bsc vállalatban
        /// </summary>
        public string DefaultPriceGroupIdBsc { set; get; }

        /// <summary>
        /// árbesorolás hrp vállalatban
        /// </summary>
        public string DefaultPriceGroupIdHrp { set; get; }

        /// <summary>
        /// képviselők lista
        /// </summary>
        public Representatives Representatives { set; get; }

        /// <summary>
        /// látogatás bejegyzés akkor érvényes, ha a lejárat ideje nagyobb mint az aktuális dátum - idő értéke
        /// </summary>
        public bool Valid { set; get; }

        public Visitor(int id, string visitorId, string loginIP, long recId, string customerId, string customerName, string personId, string personName, string email, bool isWebAdministrator, bool invoiceInfoEnabled,
                       bool priceListDownloadEnabled, bool canOrder, bool recieveGoods, string paymTermIdBsc, string paymTermIdHrp, string currency,
                       string languageId, string defaultPriceGroupIdHrp, string defaultPriceGroupIdBsc, string inventLocationIdHrp, string inventLocationIdBsc,
                       string representativeId, string representativeName, string representativePhone, string representativeMobile, string representativeExtension, string representativeEmail,
                       int loginType, bool rightHrp, bool rightBsc, bool contractHrp, bool contractBsc, int cartId, string registrationId, bool isCatalogueOpened, bool isShoppingCartOpened,
                       bool autoLogin, DateTime loginDate, DateTime logoutDate, DateTime expireDate, bool valid)
        {
            this.Id = id;
            this.VisitorId = visitorId;
            this.LoginIP = loginIP;
            this.RecId = recId;
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.PersonId = personId;
            this.PersonName = personName;
            this.Email = email;
            this.Permission = new Permission(isWebAdministrator, invoiceInfoEnabled, priceListDownloadEnabled, canOrder, recieveGoods);
            this.PaymTermIdBsc = paymTermIdBsc;
            this.PaymTermIdHrp = paymTermIdHrp;
            this.Currency = currency;
            this.LanguageId = languageId;
            this.DefaultPriceGroupIdHrp = defaultPriceGroupIdHrp;
            this.DefaultPriceGroupIdBsc = defaultPriceGroupIdBsc;
            this.InventLocationIdHrp = inventLocationIdHrp;
            this.InventLocationIdBsc = inventLocationIdBsc;
            this.Representatives = new Representatives(new Representative(representativeId, representativeName, representativePhone, representativeMobile, representativeExtension, representativeEmail));
            this.LoginType = (LoginType)loginType;
            this.RightHrp = rightHrp;
            this.RightBsc = rightBsc;
            this.ContractHrp = contractHrp;
            this.ContractBsc = contractBsc;
            this.CartId = cartId;
            this.RegistrationId = registrationId;
            this.IsCatalogueOpened = isCatalogueOpened;
            this.IsShoppingCartOpened = isShoppingCartOpened;
            this.AutoLogin = autoLogin;
            this.LoginDate = loginDate;
            this.LogoutDate = logoutDate;
            this.ExpireDate = expireDate;
            this.Valid = valid;
        }

        /// <summary>
        /// létrehozás a bejelentkezés eredményeként létező visitorDataList-ből
        /// </summary>
        /// <param name="visitorDataList"></param>
        public Visitor(List<CompanyGroup.Domain.PartnerModule.VisitorData> visitorDataList)
        {
            //this.visitorDataList = visitorDataList;

            VisitorData visitorDataHrp = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp)) : new VisitorData();

            visitorDataHrp = visitorDataHrp ?? new VisitorData();

            VisitorData visitorDataBsc = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc)) : new VisitorData();

            visitorDataBsc = visitorDataBsc ?? new VisitorData();

            this.SetVisitor(visitorDataHrp, visitorDataBsc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitorDataHrp"></param>
        /// <param name="visitorDataBsc"></param>
        private void SetVisitor(CompanyGroup.Domain.PartnerModule.VisitorData visitorDataHrp, CompanyGroup.Domain.PartnerModule.VisitorData visitorDataBsc)
        {
            /*
                    public string VisitorId { get; set; }
                    public bool LoggedIn { get; set; }

                    public int CartId { get; set; }
                    public string RegistrationId { get; set; }
                    public bool IsCatalogueOpened { get; set; }
                    public bool IsShoppingCartOpened { get; set; }
             */
            this.RightHrp = (visitorDataHrp.RightHrp || visitorDataBsc.RightHrp); // && (visitorDataHrp.ContractHrp || visitorDataBsc.ContractHrp);

            this.RightBsc = (visitorDataHrp.RightBsc || visitorDataBsc.RightBsc); //&& (visitorDataHrp.ContractBsc || visitorDataBsc.ContractBsc);

            this.ContractHrp = visitorDataHrp.ContractHrp || visitorDataBsc.ContractHrp;

            this.ContractBsc = visitorDataHrp.ContractBsc || visitorDataBsc.ContractBsc;

            this.AutoLogin = visitorDataHrp.AutoLogin || visitorDataBsc.AutoLogin;

            this.Currency = !String.IsNullOrEmpty(visitorDataHrp.Currency) ? visitorDataHrp.Currency : visitorDataBsc.Currency;

            this.CustomerId = !String.IsNullOrEmpty(visitorDataHrp.CustomerId) ? visitorDataHrp.CustomerId : visitorDataBsc.CustomerId;

            this.CustomerName = !String.IsNullOrEmpty(visitorDataHrp.CustomerName) ? visitorDataHrp.CustomerName : visitorDataBsc.CustomerName;

            //this.CustomerPriceGroups = visitorData.CustomerPriceGroups;

            this.DefaultPriceGroupIdBsc = visitorDataBsc.DefaultPriceGroupId;

            this.DefaultPriceGroupIdHrp = visitorDataHrp.DefaultPriceGroupId;

            this.Email = !String.IsNullOrEmpty(visitorDataHrp.Email) ? visitorDataHrp.Email : visitorDataBsc.Email;

            this.ExpireDate = (visitorDataHrp.ExpireDate != null) ? visitorDataHrp.ExpireDate : visitorDataBsc.ExpireDate;

            this.Id = (visitorDataHrp.Id > 0) ? visitorDataHrp.Id : visitorDataBsc.Id;

            this.InventLocationIdHrp = visitorDataHrp.InventLocationId;

            this.InventLocationIdBsc = visitorDataBsc.InventLocationId;

            this.LoginDate = (visitorDataHrp.LoginDate != null) ? visitorDataHrp.LoginDate : visitorDataBsc.LoginDate;

            this.LoginIP = !String.IsNullOrEmpty(visitorDataHrp.LoginIP) ? visitorDataHrp.LoginIP : visitorDataBsc.LoginIP;

            this.LoginType = (visitorDataHrp.LoginType >= visitorDataBsc.LoginType) ? visitorDataHrp.LoginType : visitorDataBsc.LoginType;

            this.LogoutDate = (visitorDataHrp.LogoutDate != null) ? visitorDataHrp.LogoutDate : visitorDataBsc.LogoutDate;

            this.PersonId = !String.IsNullOrEmpty(visitorDataHrp.PersonId) ? visitorDataHrp.PersonId : visitorDataBsc.PersonId;

            this.PersonName = !String.IsNullOrEmpty(visitorDataHrp.PersonName) ? visitorDataHrp.PersonName : visitorDataBsc.PersonName;

            this.RecId = (visitorDataHrp.RecId > 0) ? visitorDataHrp.RecId : visitorDataBsc.RecId;

            this.Status = (visitorDataHrp.Status >= visitorDataBsc.Status) ? visitorDataHrp.Status : visitorDataBsc.Status;

            this.Valid = (visitorDataHrp.Valid) ? visitorDataHrp.Valid : visitorDataBsc.Valid;

            this.VisitorId = !String.IsNullOrEmpty(visitorDataHrp.VisitorId) ? visitorDataHrp.VisitorId : visitorDataBsc.VisitorId;

            this.PaymTermIdHrp = visitorDataHrp.PaymTermId;

            this.PaymTermIdBsc = visitorDataBsc.PaymTermId;

            //jogosultság beállítás - 
            this.Permission = CreatePermission(visitorDataHrp.Permission, visitorDataBsc.Permission);

            this.Representatives = CreateRepresentatives(visitorDataHrp.Representative, visitorDataBsc.Representative);

            //hrp beállításait részesíti előnyben
            this.LanguageId = String.IsNullOrEmpty(visitorDataHrp.LanguageId) ? visitorDataBsc.LanguageId : visitorDataHrp.LanguageId;

            this.CartId = (visitorDataHrp.CartId > 0) ? visitorDataHrp.CartId : visitorDataBsc.CartId;

            this.RegistrationId = String.IsNullOrEmpty(visitorDataHrp.RegistrationId) ? visitorDataBsc.RegistrationId : visitorDataHrp.RegistrationId;
        }

        //private VisitorData FindVisitorData(string dataAreaId)
        //{
        //    VisitorData result = null;

        //    if (visitorDataList.Count > 0)
        //    {
        //        result = visitorDataList.Find(x => x.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp));

        //        if (result == null)
        //        {
        //            result = visitorDataList.Find(x => x.DataAreaId.Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc));
        //        }
        //    }

        //    return (result == null) ? new VisitorData() : result;
        //}

        /// <summary>
        /// bejelentkezés hívás eredménye
        /// </summary>
        //private List<CompanyGroup.Domain.PartnerModule.VisitorData> visitorDataList;

        #region "Permission"

        ///// <summary>
        ///// hrp vállalat jogosultság beállítás
        ///// </summary>
        //private Permission GetPermissionHrp()
        //{
        //    return GetPermission(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);
        //}

        ///// <summary>
        ///// bsc vállalat jogosultság beállítás
        ///// </summary>
        //private Permission GetPermissionBsc()
        //{
        //    return GetPermission(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);
        //}

        //private Permission GetPermission(string dataAreaId)
        //{
        //    VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId)) : null;

        //    return (visitorData == null) ? new Permission() : visitorData.Permission;
        //}

        /// <summary>
        /// a nagyobb jogosultságbeállítás lesz érvényben
        /// </summary>
        /// <param name="permissionBsc"></param>
        /// <param name="permissionHrp"></param>
        /// <returns></returns>
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

        ///// <summary>
        ///// bsc fizetési feltételek beállítás
        ///// </summary>
        //private string GetPaymTermIdBsc()
        //{
        //    return GetPaymTermId(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);
        //}

        ///// <summary>
        ///// hrp fizetési feltételek beállítás
        ///// </summary>
        //private string GetPaymTermIdHrp()
        //{
        //    return GetPaymTermId(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);
        //}

        //private string GetPaymTermId(string dataAreaId)
        //{
        //    VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId)) : null;

        //    return (visitorData == null) ? String.Empty : visitorData.PaymTermId;
        //}

        /// <summary>
        /// fizetési feltételek kalkuláció 
        /// ha nem üres az AX-ben a BSC és a HRP vállalatban a fizetési feltételek beállítás, akkor ellenörzést történik az ATUT beállítás meglétére.
        /// Ha van a hrp vállalatban ATUT fizetési feltétel, akkor a HRP beállítás lesz érvényben, ha a hrp-ben nincs, de a bsc-ben van, akkor a bsc fizetési feltételek beállítása lesz érvényben.
        /// Ha sem a hrp, sem pedig a bsc vállalatban nincs fizetési feltétel beállítás, akkor a KP mód kerül visszaadásra 
        /// </summary>
        public string PaymTermId
        {
            get
            {
                if (!String.IsNullOrEmpty(this.PaymTermIdBsc) && !String.IsNullOrEmpty(this.PaymTermIdHrp))
                {
                    return this.PaymTermIdHrp.StartsWith("ATUT") ? this.PaymTermIdHrp : this.PaymTermIdBsc;
                }
                if (String.IsNullOrEmpty(this.PaymTermIdBsc) && !String.IsNullOrEmpty(this.PaymTermIdHrp))
                {
                    return this.PaymTermIdHrp;
                }
                if (!String.IsNullOrEmpty(this.PaymTermIdBsc) && String.IsNullOrEmpty(this.PaymTermIdHrp))
                {
                    return this.PaymTermIdBsc;
                }
                return CompanyGroup.Domain.Core.Constants.PaymentIdKP;
            }
        }

        /// <summary>
        /// ha az AX-ben a BSC vagy a HRP vállalatban van ATUT fizetési mód beállítva, akkor igazat ad vissza, ha nincs, akkor hamisat.
        /// </summary>
        public bool PaymtermTransferEnabled
        {
            get { return this.PaymTermIdHrp.StartsWith("ATUT") || this.PaymTermIdBsc.StartsWith("ATUT"); }
        }

        #endregion

        #region "Representative"

        ///// <summary>
        ///// hrp vállalat képviselő beállítás
        ///// </summary>
        //public Representative GetRepresentativeHrp()
        //{
        //    return GetRepresentative(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);
        //}

        ///// <summary>
        ///// bsc vállalat képviselő beállítás
        ///// </summary>
        //public Representative GetRepresentativeBsc()
        //{
        //    return GetRepresentative(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);
        //}

        //private Representative GetRepresentative(string dataAreaId)
        //{
        //    VisitorData visitorData = (visitorDataList.Count > 0) ? visitorDataList.Find(x => x.DataAreaId.Equals(dataAreaId)) : null;

        //    return (visitorData == null) ? new Representative() : visitorData.Representative;
        //}

        /// <summary>
        /// ha van hrp-s kapcsolattartó és nincs bsc-s, akkor a hrp-t adja vissza. 
        /// ha van bsc-s kapcsolattartó és nincs hrp-s, akkor a bsc-t adja vissza. 
        /// ha van hrp-s és bsc-s, akkor 
        /// </summary>
        /// <param name="representativeBsc"></param>
        /// <param name="representativeHrp"></param>
        /// <returns></returns>
        private Representatives CreateRepresentatives(CompanyGroup.Domain.PartnerModule.Representative representativeBsc, CompanyGroup.Domain.PartnerModule.Representative representativeHrp)
        {
            //nem egyezik a hrp-s képviselő a bsc-s képviselővel
            if (!representativeHrp.Id.Equals(representativeBsc.Id, StringComparison.OrdinalIgnoreCase) && !String.IsNullOrEmpty(representativeHrp.Id) && !String.IsNullOrEmpty(representativeBsc.Id))
            {
                return new Representatives(new Representative(representativeHrp.Id, representativeHrp.Name, CreatePhoneNumber(representativeHrp.Phone, representativeHrp.Extension), representativeHrp.Mobile, representativeHrp.Extension, representativeHrp.Email), 
                                           new Representative(representativeBsc.Id, representativeBsc.Name, CreatePhoneNumber(representativeBsc.Phone, representativeBsc.Extension), representativeBsc.Mobile, representativeBsc.Extension, representativeBsc.Email));
            }

            if (!String.IsNullOrEmpty(representativeHrp.Id))
            {
                return new Representatives(new Representative(representativeHrp.Id, representativeHrp.Name, CreatePhoneNumber(representativeHrp.Phone, representativeHrp.Extension), representativeHrp.Mobile, representativeHrp.Extension, representativeHrp.Email));
            }
            if (!String.IsNullOrEmpty(representativeBsc.Id))
            {
                return new Representatives(new Representative(representativeBsc.Id, representativeBsc.Name, representativeBsc.Phone, representativeBsc.Mobile, representativeBsc.Extension, representativeBsc.Email));
            }
            return new Representatives();
        }

        /// <summary>
        /// telefonszám előállítása 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
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

            this.LoggedIn = DateTime.Now.CompareTo(this.ExpireDate) < 1 
                            && (loginType > 0) 
                            && (personalLoginOK || companyLoginOK)
                            && (this.RightHrp || this.RightBsc)
                            && (this.ContractHrp || this.ContractBsc)
                            && (this.Status == LoginStatus.Active || this.Status == LoginStatus.Permanent); // && this.Valid;
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
        /// 
        /// </summary>
        /// <param name="a">termék adata</param>
        /// <param name="b">árbesorolás</param>
        /// <returns></returns>
        private static bool CompareItems(string a, string b)
        {
            return a.Equals(b, StringComparison.OrdinalIgnoreCase) || b.Equals("____", StringComparison.OrdinalIgnoreCase) || String.IsNullOrEmpty(b);
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
        public decimal CalculateCustomerPrice(decimal price1, decimal price2, decimal price3, decimal price4, decimal price5,
                                              string manufacturerId, string category1Id, string category2Id, string category3Id, string dataAreaId)
        {
            //szűrés vállalatkódra, azaz csak azok az árcsoprotok kerülnek az eredményhalmazba, melyek vállalatkódja egyezik a termék vállalatkódjával
            IEnumerable<CustomerPriceGroup> priceGroups = this.CustomerPriceGroups.Where(x => (x.DataAreaId.Equals(dataAreaId, StringComparison.OrdinalIgnoreCase)));

            //szűrt lista másolása
            List<CustomerPriceGroup> priceGroupList = new List<CustomerPriceGroup>();

            //másolás érték szerint
            foreach (CustomerPriceGroup p in priceGroups)
            {
                if (CompareItems(category1Id, p.Category1Id) && CompareItems(category2Id, p.Category2Id) && CompareItems(category3Id, p.Category3Id) && CompareItems(manufacturerId, p.ManufacturerId))
                {
                    priceGroupList.Add(new CustomerPriceGroup(p.LineId, p.VisitorKey, p.PriceGroupId, p.ManufacturerId, p.Category1Id, p.Category2Id, p.Category3Id, p.Order, p.DataAreaId));
                }
            }

            CustomerPriceGroup priceGroup;

            if (priceGroupList.Count > 0)
            {
                //sorba rendezés 1..n -ig, legelső elem kiolvasása
                priceGroup = priceGroupList.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //kikeresés nem volt sikeres, ezért a vevő alapértelmezett árát kell visszaadni
            string defaultPriceGroup = (dataAreaId.Equals(Core.Constants.DataAreaIdHrp)) ? this.DefaultPriceGroupIdHrp : this.DefaultPriceGroupIdBsc;

            return LookupPrice(price1, price2, price3, price4, price5, (String.IsNullOrEmpty(defaultPriceGroup) ? "2" : defaultPriceGroup));
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
        public decimal CalculateCustomerPrice2(decimal price1, decimal price2, decimal price3, decimal price4, decimal price5,
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
        /// hrp vállalatban jogosultság létezik-e?
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorizedInHrp()
        {
            return this.RightHrp;
        }

        /// <summary>
        /// bsc vállalatban jogosultság létezik-e?
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorizedInBsc()
        {
            return this.RightBsc;
        }

        /// <summary>
        /// visszaadja azt a vállalatkód listát, melyben a bejelentkezett felhasználó megtalálható
        /// (ha az egyik vállalatban megtalálható, akkor benne kell legyen a másikban is)
        /// </summary>
        /// <returns></returns>
        public List<string> AuthorizedDataAreaList()
        {
            List<string> dataAreaList = new List<string>();

            dataAreaList.Add(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

            dataAreaList.Add(CompanyGroup.Domain.Core.Constants.DataAreaIdBsc);

            return dataAreaList;
        }

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (this.IsTransient())
            {
                validationResults.Add(new System.ComponentModel.DataAnnotations.ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "ObjectId" }));
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
