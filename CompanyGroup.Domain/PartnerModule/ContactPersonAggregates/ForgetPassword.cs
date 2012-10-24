using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// jelszómódosítás
    /// </summary>
    public class ForgetPassword : CompanyGroup.Domain.Core.NoSqlEntity, IValidatableObject
    {
        [MongoDB.Bson.Serialization.Attributes.BsonElement("UserName", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string UserName { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Status", Order = 5)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(1)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public ForgetPasswordStatus Status { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("CreatedDate", Order = 6)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("0000.00.00 00:00:00")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public DateTime CreatedDate { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("DataAreaId", Order = 7)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string DataAreaId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (String.IsNullOrWhiteSpace(this.UserName))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.verification_UserNameCannotBeNull, new string[] { "UserName" }));
            }

            if (String.IsNullOrWhiteSpace(this.DataAreaId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.verification_DataAreaIdCannotBeNull, new string[] { "DataAreaId" }));
            }

            return validationResults;
        }

    }
}
