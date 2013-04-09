using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    public class Visitor
    {
        public Visitor()
        {
            this.Id = String.Empty;
            this.CompanyId = String.Empty;
            this.CompanyName = String.Empty;
            this.PersonId = String.Empty;
            this.PersonName = String.Empty;
            this.LoggedIn = false;
            this.IsValidLogin = false;
            this.Permission = new Permission();
            this.Roles = new List<string>();
            this.History = new List<string>();
            this.Representative = new Representative();
            this.PaymTermId = String.Empty;
            this.Currency = String.Empty;
            this.InventLocationBsc = String.Empty;
            this.InventLocationHrp = String.Empty;
            this.LanguageId = String.Empty;
            this.BscAuthorized = false;
            this.HrpAuthorized = false;
        }

        public string Id { get; set; }

        /// <summary>
        /// vállalat azonosítója
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// vállalat neve
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// személy azonosítója
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// személy neve
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// belépés megtörtént-e
        /// </summary>
        public bool LoggedIn { get; set; }

        /// <summary>
        /// belépés megtörtént-e
        /// </summary>
        public bool IsValidLogin { get; set; }

        /// <summary>
        /// látogató jogosultsági beállításai
        /// </summary>
        public Permission Permission { get; set; }

        /// <summary>
        /// szerepkörök
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// látogatott oldalak url listája
        /// </summary>
        public List<string> History { get; set; }

        /// <summary>
        /// fizetési feltételek
        /// </summary>
        public string PaymTermId { set; get; }

        /// <summary>
        /// alapértelmezett valutanem
        /// </summary>
        public string Currency { set; get; }

        /// <summary>
        /// alapértelmezett raktár bsc
        /// </summary>
        public string InventLocationBsc { set; get; }

        /// <summary>
        /// alapértelmezett raktár hrp
        /// </summary>
        public string InventLocationHrp { set; get; }

        /// <summary>
        /// alapértelmezett nyelv
        /// </summary>
        public string LanguageId { set; get; }

        /// <summary>
        /// képviselő
        /// </summary>
        public Representative Representative { set; get; }

        /// <summary>
        /// jogosult a HRP-ben vásárolni
        /// </summary>
        public bool HrpAuthorized { get; set; }

        /// <summary>
        /// jogosult a BSC-ben vásárolni
        /// </summary>
        public bool BscAuthorized { get; set; }
    }
}
