using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// összehasonlítható elem
    /// </summary>
    public class ComparableItem : CompanyGroup.Domain.Core.NoSqlEntity, IValidatableObject
    {
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Product", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Product Product { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("VisitorId", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string VisitorId { get; set; }

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (String.IsNullOrEmpty(this.Product.ProductId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "ProductId" }));
            }

            if (String.IsNullOrEmpty(this.VisitorId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "VisitorId" }));
            }

            return validationResults;
        }
    }
}
