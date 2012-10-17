using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.PartnerModule
{
    public class ProductManager : IValidatableObject
    {
        public ProductManager() : this("", "", "", "", "") { }

        public ProductManager(string employeeId, string name, string email, string extension, string mobile)
        {
            this.EmployeeId = employeeId; 
            this.Name = name; 
            this.Email = email; 
            this.Extension = extension; 
            this.Mobile = mobile;
        }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("EmployeeId", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string EmployeeId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Name", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Name { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Email", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Email { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Extension", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Extension { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Mobile", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Mobile { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (String.IsNullOrEmpty(this.EmployeeId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ProductManagerIdCannotBeNullOrEmpty, new string[] { "ProductManager.EmployeeId" }));
            }

            if (String.IsNullOrEmpty(this.Name))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ProductManagerNameCannotBeNullOrEmpty, new string[] { "ProductManager.Name" }));
            }

            return validationResults;
        }
    }
}
