using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// termék kategória
    /// </summary>
    public class Category : CompanyGroup.Domain.Core.ValueObject<Category>
    {
        public Category(string categoryId, string categoryName, string categoryEnglishName)
        {
            this.CategoryId = categoryId;

            this.CategoryName = categoryName;

            this.CategoryEnglishName = categoryEnglishName;
        }

        public Category() : this(String.Empty, String.Empty, String.Empty) { }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryEnglishName { get; set; }
    }
}
