using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// Kijelentkezett, vagy bejelentkezett látogató entitás
    /// </summary>
    public class Visitor : CompanyGroup.Domain.Core.Entity, IValidatableObject
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
        /// profil kiolvasás, beállítás
        /// </summary>
        //public CompanyGroup.Domain.PartnerModule.Profile Profile { get; private set; }
        public IList<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> CustomerPriceGroups { get; set; }

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
        /// alapértelmezett nyelv
        /// </summary>
        public string LanguageId { set; get; }

        /// <summary>
        /// árbesorolás 
        /// </summary>
        public string DefaultPriceGroupId { set; get; }

        /// <summary>
        /// képviselő
        /// </summary>
        public Representative Representative { set; get; }

        /// <summary>
        /// látogatás bejegyzés akkor érvényes, ha a lejárat ideje nagyobb mint az aktuális dátum - idő értéke
        /// </summary>
        public bool Valid { set; get; }

        /// <summary>
        /// bejelentkezés lejárt-e, vagy sem?
        /// </summary>
        public bool IsValidLogin
        {
            get { return (DateTime.Now.CompareTo(this.ExpireDate) < 1) && (this.LoggedIn) && (this.Status == LoginStatus.Active || this.Status == LoginStatus.Permanent); }
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
        /// profile beállítása
        /// </summary>
        //public void SetProfile(CompanyGroup.Domain.PartnerModule.Profile profile)
        //{

        //    if (profile == null)
        //    {
        //        throw new ArgumentNullException("profile");
        //    }

        //    this.Profile = profile;
        //}

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
        /// <returns></returns>
        public decimal CalculateCustomerPrice(decimal price1, decimal price2, decimal price3, decimal price4, decimal price5,
                                              string manufacturerId, string category1Id, string category2Id, string category3Id)
        {
            //Amiből nincs 2-es ára a vevőnek, azok a belépéskor a profil-ba kerülnek. Ebből a listából ki kell keresni a termékhez tartozó elemet.
            this.CustomerPriceGroups.Where(x =>
                                    (x.ManufacturerId == manufacturerId || String.IsNullOrEmpty(x.ManufacturerId)) &&
                                    (x.Category1Id == category1Id || String.IsNullOrEmpty(x.Category1Id)) &&
                                    (x.Category2Id == category2Id || String.IsNullOrEmpty(x.Category2Id)) &&
                                    (x.Category3Id == category3Id || String.IsNullOrEmpty(x.Category3Id)));

            //sorba rendezés 1..n -ig
            this.CustomerPriceGroups.OrderBy(x => x.Order);

            //legkisebb árcsoport szerinti kiválasztás történik. Az AX szöveges formában tárolja az árcsoportokat
            string priceGroup = this.CustomerPriceGroups.Select(x => x.PriceGroupId).FirstOrDefault();

            //decimal priceGroup = 0;

            //if (Decimal.TryParse(s, out priceGroup))
            //{
                if (priceGroup.Equals("1")) { return price1; }
                if (priceGroup.Equals("2")) { return price2; }
                if (priceGroup.Equals("3")) { return price3; }
                if (priceGroup.Equals("4")) { return price4; }
                if (priceGroup.Equals("5")) { return price5; }

                //return price2;
            //}

            return price2;
        }

        public bool IsAuthorizedInHrp()
        {
            return this.PartnerModel.Equals(PartnerModel.Hrp) || this.PartnerModel.Equals(PartnerModel.Both);
        }

        public bool IsAuthorizedInBsc()
        {
            return this.PartnerModel.Equals(PartnerModel.Bsc) || this.PartnerModel.Equals(PartnerModel.Both);
        }

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
    }
}
