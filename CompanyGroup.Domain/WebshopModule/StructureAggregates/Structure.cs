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
        public Structure(string manufacturerId, string manufacturerName, string manufacturerEnglishName,
                         string category1Id, string category1Name, string category1EnglishName,
                         string category2Id, string category2Name, string category2EnglishName,
                         string category3Id, string category3Name, string category3EnglishName)
        {
            this.Manufacturer = new Manufacturer(manufacturerId, manufacturerName, manufacturerEnglishName);

            this.Category1 = new Category(category1Id, category1Name, category1EnglishName);

            this.Category2 = new Category(category2Id, category2Name, category2EnglishName);

            this.Category3 = new Category(category3Id, category3Name, category3EnglishName);
        }

        public Structure() : this(String.Empty, String.Empty, String.Empty,
                                  String.Empty, String.Empty, String.Empty,
                                  String.Empty, String.Empty, String.Empty,
                                  String.Empty, String.Empty, String.Empty)
        { }

        public Manufacturer Manufacturer { get; set; }

        public Category Category1 { get; set; }

        public Category Category2 { get; set; }

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
