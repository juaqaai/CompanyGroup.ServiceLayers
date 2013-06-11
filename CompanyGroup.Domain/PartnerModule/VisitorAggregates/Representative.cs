using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    public class Representative : CompanyGroup.Domain.Core.ValueObject<Representative>
    {
        public Representative(string id, string name, string phone, string mobile, string extension, string email)
        { 
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.Mobile = mobile;
            this.Extension = extension;
            this.Email = email;
        }

        public Representative() : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty) { }

        /// <summary>
        /// képviselő azonosító
        /// </summary>
        public string Id { set; get; }

        /// <summary>
        /// képviselő neve
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// képviselő telefon
        /// </summary>
        public string Phone { set; get; }

        /// <summary>
        /// képviselő mobil telefon
        /// </summary>
        public string Mobile { set; get; }

        /// <summary>
        /// képviselő mellék
        /// </summary>
        public string Extension { set; get; }

        /// <summary>
        /// képviselő email cím
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// alapérelmezett adatok 
        /// </summary>
        public void SetDefault()
        {
            if (String.IsNullOrEmpty(this.Id) && String.IsNullOrEmpty(this.Name) && String.IsNullOrEmpty(this.Phone) && String.IsNullOrEmpty(this.Mobile) && String.IsNullOrEmpty(this.Extension) && String.IsNullOrEmpty(this.Email))
            {
                this.Id = "";
                this.Name = "telesales";
                this.Phone = "+36 1 452 4600";
                this.Mobile = "";
                this.Extension = "";
                this.Email = "telesales@hrp.hu";
            }
        }
    }

    /// <summary>
    /// képviselők (lehetséges, hogy a hrp és a bsc képviselő nem ugyanaz)
    /// </summary>
    public class Representatives : List<Representative>
    {
        public Representatives(Representative representative)
        {
            this.Add(representative);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="representativeBsc"></param>
        /// <param name="representativeHrp"></param>
        public Representatives(Representative representativeBsc, Representative representativeHrp)
        {
            this.Add(representativeBsc);

            this.Add(representativeHrp);
        }

        public Representatives() { }

        /// <summary>
        /// képviselő alapértelmezett értékre történő beállítása
        /// </summary>
        public void SetDefaults()
        {
            this.ForEach( x => x.SetDefault());
        }
    }
}
