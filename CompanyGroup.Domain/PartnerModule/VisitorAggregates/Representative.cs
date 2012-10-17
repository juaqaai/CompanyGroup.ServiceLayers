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
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Id", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Id { set; get; }

        /// <summary>
        /// képviselő neve
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Name", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]        
        public string Name { set; get; }

        /// <summary>
        /// képviselő telefon
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Phone", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]        
        public string Phone { set; get; }

        /// <summary>
        /// képviselő mobil telefon
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Mobile", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Mobile { set; get; }

        /// <summary>
        /// képviselő mellék
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Extension", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]        
        public string Extension { set; get; }

        /// <summary>
        /// képviselő email cím
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Email", Order = 6)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]        
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
}
