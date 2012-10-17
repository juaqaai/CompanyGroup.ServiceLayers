using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// Kijelentkezett, vagy bejelentkezett látogató entitás
    /// </summary>
    public class Visitor : CompanyGroup.Domain.Core.NoSqlEntity, IValidatableObject
    {
        //private LoginInfo loginInfo;

        public Visitor(CompanyGroup.Domain.PartnerModule.LoginInfo loginInfo)
        {
            //this.loginInfo = loginInfo;

            this.CompanyId = loginInfo.CompanyId;

            this.CompanyName = loginInfo.CompanyName;

            this.PersonId = loginInfo.PersonId;

            this.PersonName = loginInfo.PersonName;

            this.Email = loginInfo.Email;

            this.LoginType = loginInfo.LoginType;

            //this.SetLoggedIn(this.LoginType);

            this.PartnerModel = loginInfo.PartnerModel;

            this.Permission = new CompanyGroup.Domain.PartnerModule.Permission(loginInfo.IsWebAdministrator, loginInfo.InvoiceInfoEnabled, loginInfo.PriceListDownloadEnabled, loginInfo.CanOrder, loginInfo.RecieveGoods);

            this.Profile = new Profile();

            this.CreatedDate = DateTime.MinValue;

            this.ExpiredDate = DateTime.MinValue;

            this.Status = LoginStatus.Active;

            this.DataAreaId = String.Empty; 

            this.IPAddress = String.Empty;

            this.PaymTermId = loginInfo.PaymTermId;

            this.Currency = loginInfo.Currency;

            this.InventLocation = loginInfo.InventLocation;

            this.LanguageId = loginInfo.LanguageId;

            this.Representative = new Representative( loginInfo.RepresentativeId, loginInfo.RepresentativeName, loginInfo.RepresentativePhone, loginInfo.RepresentativeMobile, loginInfo.RepresentativeExtension, loginInfo.RepresentativeEmail );

            this.PriceGroup = loginInfo.PriceGroup;

        }

        public Visitor() : this(new LoginInfo("", "", "", "", "", false, false, false, false, false, 0, 0, "", "", "", "", 0, "", "", "", "", "", "")) { }

        /// <summary>
        /// vállalat azonosítója
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CompanyId", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CompanyId { get; set; }

        /// <summary>
        /// vállalat neve
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CompanyName", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string CompanyName { get; set; }

        /// <summary>
        /// személy azonosítója
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("PersonId", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string PersonId { get; set; }

        /// <summary>
        /// személy neve
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("PersonName", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string PersonName { get; set; }

        /// <summary>
        /// bejelentkezés típusától függően vagy a kapcsolattartó email címe, vagy a cég email címe
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Email", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Email { get; set; }

        /// <summary>
        /// bejelentkezett-e, vagy sem
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public bool LoggedIn { get; set; }

        /// <summary>
        /// bejelentkezett-e tulajdonság beállítása beállító metóduson keresztül       
        /// </summary>
        public void SetLoggedIn()
        {
            this.SetLoggedIn(this.LoginType);
        }

        /// <summary>
        /// bejelentkezett-e tulajdonság beállítása beállító metóduson keresztül       
        /// </summary>
        /// <param name="loginType"></param>
        public void SetLoggedIn(LoginType loginType)
        { 
            bool personalLoginOK = (loginType == LoginType.Person) && (!String.IsNullOrWhiteSpace(this.CompanyId)) && (!String.IsNullOrWhiteSpace(this.CompanyName))  && (!String.IsNullOrWhiteSpace(this.PersonId)) && (!String.IsNullOrWhiteSpace(this.PersonName));

            bool companyLoginOK = (loginType == LoginType.Company) && (!String.IsNullOrWhiteSpace(this.CompanyId)) && (!String.IsNullOrWhiteSpace(this.CompanyName));

            this.LoggedIn = DateTime.Now.CompareTo(this.ExpiredDate) < 1 && (loginType > 0) && (personalLoginOK || companyLoginOK) && (this.Status == LoginStatus.Active || this.Status == LoginStatus.Permanent);
        }

        /// <summary>
        /// bejelentkezés típusa (None = 0, Company = 1, Person = 2)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("LoginType", Order = 6)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public LoginType LoginType { get; set; }

        /// <summary>
        /// profil kiolvasás, beállítás
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Profile", Order = 7)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public CompanyGroup.Domain.PartnerModule.Profile Profile { get; private set; }

        /// <summary>
        /// jogosultság beállítás
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Permission", Order = 8)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Permission Permission { get; set; }

        /// <summary>
        /// szerepkörök beállítása a jogosultságkezeléshez
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
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
        }

        /// <summary>
        /// partnermodel (None = 0, Hrp = 1, Bsc = 2, Both = 3)
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("PartnerModel", Order = 9)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public PartnerModel PartnerModel { get; set; }

        /// <summary>
        /// Visitor bejegyzés keletkezésének időpontja
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("CreatedDate", Order = 10)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("0000.00.00 00:00:00")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime CreatedDate { set; get; }

        /// <summary>
        /// Visitor bejegyzés érvényességének időpontja
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ExpiredDate", Order = 11)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("0000.00.00 00:00:00")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime ExpiredDate { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Status", Order = 12)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public LoginStatus Status { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("DataAreaId", Order = 13)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string DataAreaId { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("IPAddress", Order = 14)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string IPAddress { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("PaymTermId", Order = 15)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string PaymTermId { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Currency", Order = 16)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string Currency { set; get; }
 
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InventLocation", Order = 17)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string InventLocation { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("LanguageId", Order = 18)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        public string LanguageId { set; get; }

        /// <summary>
        /// árbesorolás 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("PriceGroup", Order = 19)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]        
        public int PriceGroup { set; get; }

        /// <summary>
        /// képviselő
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Representative", Order = 20)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Representative Representative { set; get; }

        /// <summary>
        /// látogatás bejegyzés akkor érvényes, ha a lejárat ideje nagyobb mint az aktuális dátum - idő értéke
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public bool IsValid 
        {
            get { return DateTime.Now.CompareTo(this.ExpiredDate) < 1; }
        }

        /// <summary>
        /// bejelentkezés lejárt-e, vagy sem?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public bool IsValidLogin
        {
            get { return (DateTime.Now.CompareTo(this.ExpiredDate) < 1) && (this.LoggedIn) && (this.Status == LoginStatus.Active || this.Status == LoginStatus.Permanent); }
        }

        /// <summary>
        /// profile beállítása
        /// </summary>
        public void SetProfile(CompanyGroup.Domain.PartnerModule.Profile profile)
        {

            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            this.Profile = profile;
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
            this.Profile.CustomerPriceGroups.Where(x =>
                                    (x.ManufacturerId == manufacturerId || String.IsNullOrEmpty(x.ManufacturerId)) &&
                                    (x.Category1Id == category1Id || String.IsNullOrEmpty(x.Category1Id)) &&
                                    (x.Category2Id == category2Id || String.IsNullOrEmpty(x.Category2Id)) &&
                                    (x.Category3Id == category3Id || String.IsNullOrEmpty(x.Category3Id)));

            //sorba rendezés 1..n -ig
            this.Profile.CustomerPriceGroups.OrderBy(x => x.Order);

            //legkisebb árcsoport szerinti kiválasztás történik. Az AX szöveges formában tárolja az árcsoportokat
            string priceGroup = this.Profile.CustomerPriceGroups.Select(x => x.PriceGroupId).FirstOrDefault();

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
