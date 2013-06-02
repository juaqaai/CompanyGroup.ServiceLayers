using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// átmeneti látogató entitás (belépést követően ebbe az adattípusba kerülnek beolvasásra az adatok) Visitor-ra kell konvertálni
    /// </summary>
    /// <example>
    /// Id	VisitorId	LoginIP	RecId	    CustomerId	CustomerName	    PersonId	PersonName	    Email	        IsWebAdministrator	InvoiceInfoEnabled	PriceListDownloadEnabled	CanOrder	RecieveGoods	PaymTermId	Currency	LanguageId	DefaultPriceGroupId	InventLocationId	RepresentativeId	RepresentativeName	RepresentativePhone	RepresentativeMobile	RepresentativeExtension	RepresentativeEmail	DataAreaId	LoginType	RightHrp	RightBsc	ContractHrp	ContractBsc	CartId	RegistrationId	IsCatalogueOpened	IsShoppingCartOpened	AutoLogin	LoginDate	            LogoutDate	            ExpireDate	            Valid
    /// 0	                    5637231927	V001446	    iPon Computer Kft.	KAPCS03399	Parádi András	paradi@ipon.hu	0	                1	                1	                        1	        1	            ATUT21	    HUF	        hu	        3	                7000	            SipiloE	            Sipiló Erika	    452-4681			                                                esipilo@bsc.hu	    bsc	        2	        1	        1	        0	        1	        0		0	            0	                0	                                2013-05-05 21:53:06.850	1900-01-01 00:00:00.000	2013-05-06 21:53:06.850	1
    /// 0	                    5637231927	V001446	    iPon Computer Kft.	KAPCS03399	Parádi András	paradi@ipon.hu	0	                1	                1	                        1	        1	            ATUT8	    HUF	        HU	        4	                KULSO	            SipiloE	            Sipiló Erika	    452-4681			                                                esipilo@bsc.hu	    hrp	        2	        1	        1	        1	        0	        0		0	            0	                0	                                2013-05-05 21:53:06.850	1900-01-01 00:00:00.000	2013-05-06 21:53:06.850	1
    /// </example>
    public class VisitorData : CompanyGroup.Domain.Core.Entity
    {
        /// <summary>
        /// base entity miatt van int Id-ja
        /// </summary>
        public VisitorData()
        { 
            this.VisitorId = String.Empty;
            this.LoginIP = String.Empty;
            this.AutoLogin = false;
            this.RecId = 0;
            this.CustomerId = String.Empty;
            this.CustomerName = String.Empty;
            //this.CustomerPriceGroups = new List<CustomerPriceGroup>();
            this.PersonId = String.Empty;
            this.PersonName = String.Empty;
            this.Email = String.Empty;
            this.LoggedIn = false;
            this.LoginType = LoginType.None;
            this.Permission = new PartnerModule.Permission();
            this.LoginDate = DateTime.MinValue;
            this.LogoutDate = DateTime.MinValue;
            this.ExpireDate = DateTime.MinValue;
            this.Status = LoginStatus.Passive;
            this.DataAreaId = String.Empty;
            this.PaymTermId = String.Empty;
            this.Currency = String.Empty;
            this.InventLocationId = String.Empty;
            this.LanguageId = String.Empty;
            this.DefaultPriceGroupId  = String.Empty;
            this.Representative = new Representative();
            this.RightHrp = false; 
            this.RightBsc = false; 
            this.ContractHrp = false; 
            this.ContractBsc = false; 
            this.CartId = 0;
            this.RegistrationId = String.Empty; 
            this.IsCatalogueOpened = false; 
            this.IsShoppingCartOpened = false;	
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
        //public IList<CompanyGroup.Domain.PartnerModule.CustomerPriceGroup> CustomerPriceGroups { get; set; }

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

        public bool RightHrp { get; set; } 
        public bool RightBsc { get; set; } 
        public bool ContractHrp { get; set; } 
        public bool ContractBsc { get; set; } 
        public int CartId { get; set; } 
        public string RegistrationId { get; set; }
        public bool IsCatalogueOpened { get; set; }
        public bool IsShoppingCartOpened { get; set; }

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

    }

}
