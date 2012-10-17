using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// </summary>
    public class Newsletter : CompanyGroup.Domain.Core.Entity, IValidatableObject
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="htmlPath"></param>
        /// <param name="endDate"></param>
        /// <param name="picturePath"></param>
        /// <param name="allowedDate"></param>
        /// <param name="allowedTime"></param>
        public Newsletter(string id, string title, string description, string htmlPath, DateTime endDate, string picturePath, DateTime allowedDate, int allowedTime )
        { 
            this.Id = id;

            this.Title = title;

            this.Description = description;

            this.HtmlPath = htmlPath;

            this.EndDateTime = endDate; //endTime

            this.PicturePath = picturePath;

            this.AllowedDateTime = allowedDate;    //allowedTime   
        }

        /// <summary>
        /// hírlevél cím
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// rövid leírás
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// html hírlevél elérési útja
        /// </summary>
        public string HtmlPath{ get; set; }

        /// <summary>
        /// megjelenés utolsó dátuma
        /// </summary>
        public DateTime EndDateTime{ get; set; }

        /// <summary>
        /// thumbnail kép elérési útja
        /// </summary>
        public string PicturePath	{ get; set; }

        /// <summary>
        /// engedélyezés dátum - ideje
        /// </summary>
        public DateTime AllowedDateTime { get; set; }

        /// <summary>
        /// hírlevél tartalmi része
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (this.IsTransient())
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "Id" }));
            }

            return validationResults;
        }
    }
}
