using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// struktúra domain elem
    /// </summary>
    public class Structure : CompanyGroup.Domain.Core.ValueObject<Structure>, IValidatableObject
    {
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Manufacturer", Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Manufacturer Manufacturer { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Category1", Order = 2)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Category Category1 { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Category2", Order = 3)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Category Category2 { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Category3", Order = 4)]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Category Category3 { get; set; }

        #region IValidatableObject Members

        /// <summary>
        /// érvényesség ellenörzés
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            //if (String.IsNullOrEmpty(this.Manufacturer.Id) || String.IsNullOrWhiteSpace(this.Manufacturer.Id))
            //{
            //    validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ManufacturerIdCannotBeNull,
            //                                               new string[] { "ManufacturerId" }));
            //}

            return validationResults;
        }

        #endregion
    }

    /// <summary>
    /// struktúra lista domain entitás
    /// </summary>
    public class Structures : List<Structure>
    {
        public Structures(List<Structure> structures)
        {
            this.AddRange(structures);
        }
    }
}
