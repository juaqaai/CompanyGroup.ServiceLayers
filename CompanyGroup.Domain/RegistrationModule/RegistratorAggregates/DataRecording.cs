using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// regisztrációt elvégző, adatrögzítő  
    /// </summary>
    public class DataRecording : CompanyGroup.Domain.Core.ValueObject<DataRecording>
    {
        public DataRecording() : this(String.Empty, String.Empty, String.Empty) { }

        public DataRecording(string email, string name, string phone)
        {
            this.Email = email;
            this.Name = name;
            this.Phone = phone;
        }

        /// <summary>
        /// email
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Email")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Email { get; set; }

        /// <summary>
        /// név
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Name")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Name { get; set; }

        /// <summary>
        /// telefon
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Phone")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Phone { get; set; }
    }
}
