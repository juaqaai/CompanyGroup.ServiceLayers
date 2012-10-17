using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    [System.Runtime.Serialization.DataContract(Name = "Visitor", Namespace = "CompanyGroup.Dto.PartnerModule")]
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
            PaymTermId = String.Empty;
            Currency = String.Empty;
            InventLocation = String.Empty;
            LanguageId = String.Empty;
            this.BscAuthorized = false;
            this.HrpAuthorized = false;
        }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        /// <summary>
        /// vállalat azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CompanyId", Order = 2)]
        public string CompanyId { get; set; }

        /// <summary>
        /// vállalat neve
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "CompanyName", Order = 3)]
        public string CompanyName { get; set; }

        /// <summary>
        /// személy azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PersonId", Order = 4)]
        public string PersonId { get; set; }

        /// <summary>
        /// személy neve
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PersonName", Order = 5)]
        public string PersonName { get; set; }

        /// <summary>
        /// belépés megtörtént-e
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LoggedIn", Order = 6)]
        public bool LoggedIn { get; set; }

        /// <summary>
        /// belépés megtörtént-e
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "IsValidLogin", Order = 7)]
        public bool IsValidLogin { get; set; }

        /// <summary>
        /// látogató jogosultsági beállításai
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Permission", Order = 8)]
        public Permission Permission { get; set; }

        /// <summary>
        /// szerepkörök
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Roles", Order = 9)]
        public List<string> Roles { get; set; }

        /// <summary>
        /// látogatott oldalak url listája
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "History", Order = 10)]
        public List<string> History { get; set; }

        /// <summary>
        /// fizetési feltételek
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PaymTermId", Order = 11)]
        public string PaymTermId { set; get; }

        /// <summary>
        /// alapértelmezett valutanem
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Currency", Order = 12)]
        public string Currency { set; get; }

        /// <summary>
        /// alapértelmezett raktár
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "InventLocation", Order = 13)]
        public string InventLocation { set; get; }

        /// <summary>
        /// alapértelmezett nyelv
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 14)]
        public string LanguageId { set; get; }

        /// <summary>
        /// képviselő
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Representative", Order = 15)]
        public Representative Representative { set; get; }

        /// <summary>
        /// jogosult a HRP-ben vásárolni
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "HrpAuthorized", Order = 16)]
        public bool HrpAuthorized { get; set; }

        /// <summary>
        /// jogosult a BSC-ben vásárolni
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "BscAuthorized", Order = 17)]
        public bool BscAuthorized { get; set; }
    }
}
