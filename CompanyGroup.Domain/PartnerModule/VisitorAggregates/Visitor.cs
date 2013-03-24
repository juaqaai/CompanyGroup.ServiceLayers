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

            //vizsgálat teljes egyezésre, gyártó - jelleg1 - jelleg2 - jelleg3, vagy üres árbesorolás és termék gyártó - üres árbesorolás és termék jelleg1 - üres árbesorolás és termék jelleg2 - üres árbesorolás és termék jelleg3
            IEnumerable<CustomerPriceGroup> priceGroups = this.CustomerPriceGroups.Where(x =>
                                                                                        ((x.ManufacturerId == manufacturerId) || (String.IsNullOrEmpty(x.ManufacturerId) && String.IsNullOrEmpty(manufacturerId))) &&
                                                                                        ((x.Category1Id == category1Id) || (String.IsNullOrEmpty(x.Category1Id) && String.IsNullOrEmpty(category1Id))) &&
                                                                                        ((x.Category2Id == category2Id) || (String.IsNullOrEmpty(x.Category2Id) && String.IsNullOrEmpty(category2Id))) &&
                                                                                        ((x.Category3Id == category3Id) || (String.IsNullOrEmpty(x.Category3Id) && String.IsNullOrEmpty(category3Id))) &&
                                                                                        (x.DataAreaId == dataAreaId));
            
            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //vizsgálat gyártó - jelleg1 - jelleg2 egyezésre, vagy üres árbesorolás és termék  gyártó - üres árbesorolás és termék jelleg1 - üres árbesorolás és termék jelleg2
            priceGroups = this.CustomerPriceGroups.Where(x =>
                                                        ((x.ManufacturerId == manufacturerId) || (String.IsNullOrEmpty(x.ManufacturerId) && String.IsNullOrEmpty(manufacturerId))) &&
                                                        ((x.Category1Id == category1Id) || (String.IsNullOrEmpty(x.Category1Id) && String.IsNullOrEmpty(category1Id))) &&
                                                        ((x.Category2Id == category2Id) || (String.IsNullOrEmpty(x.Category2Id) && String.IsNullOrEmpty(category2Id))) &&
                                                        (x.DataAreaId == dataAreaId));

            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //vizsgálat gyártó - jelleg1 - egyezésre, vagy üres üres árbesorolás és termék gyártó - üres árbesorolás és termék jelleg1
            priceGroups = this.CustomerPriceGroups.Where(x =>
                                                        ((x.ManufacturerId == manufacturerId) || (String.IsNullOrEmpty(x.ManufacturerId) && String.IsNullOrEmpty(manufacturerId))) &&
                                                        ((x.Category1Id == category1Id) || (String.IsNullOrEmpty(x.Category1Id) && String.IsNullOrEmpty(category1Id))) &&
                                                        (x.DataAreaId == dataAreaId));

            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //vizsgálat gyártó - egyezésre, vagy üres árbesorolás és termék gyártó
            priceGroups = this.CustomerPriceGroups.Where(x => ((x.ManufacturerId == manufacturerId) || (String.IsNullOrEmpty(x.ManufacturerId) && String.IsNullOrEmpty(manufacturerId))) && (x.DataAreaId == dataAreaId));

            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //vizsgálat üres gyártó, jelleg1, jelleg2, jelleg3 -ra
            priceGroups = this.CustomerPriceGroups.Where(x => (x.ManufacturerId == "" && x.Category1Id == "" && x.Category2Id == "" && x.Category3Id == "" && x.DataAreaId == dataAreaId));

            if (priceGroups.Count() > 0)
            {
                //sorba rendezés 1..n -ig
                priceGroup = priceGroups.OrderBy(x => x.Order).FirstOrDefault();

                return LookupPrice(price1, price2, price3, price4, price5, priceGroup.PriceGroupId);
            }

            //alapártelmezett 2-es árat kell visszaadni
            return LookupPrice(price1, price2, price3, price4, price5, "2");
        }

        private decimal LookupPrice(decimal price1, decimal price2, decimal price3, decimal price4, decimal price5, string priceGroup)
        {
            if (priceGroup.Equals("1")) { return price1; }
            if (priceGroup.Equals("2")) { return price2; }
            if (priceGroup.Equals("3")) { return price3; }
            if (priceGroup.Equals("4")) { return price4; }
            if (priceGroup.Equals("5")) { return price5; }

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
