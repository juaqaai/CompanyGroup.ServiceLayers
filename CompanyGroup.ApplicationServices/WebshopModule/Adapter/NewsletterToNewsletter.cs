using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// Domain Newsletter -> DTO Newsletter 
    /// </summary>
    public class NewsletterToNewsletter
    {
        /// <summary>
        /// domain newsletter -> dto newsletter
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Newsletter Map(CompanyGroup.Domain.WebshopModule.Newsletter from)
        {
            try
            {
                return new CompanyGroup.Dto.WebshopModule.Newsletter()
                           {
                               AllowedDateTime = String.Format("{0}.{1}.{2}", from.AllowedDateTime.Year, from.AllowedDateTime.Month, from.AllowedDateTime.Day),
                               Body = from.Body,
                               Description = from.Description,
                               EndDateTime = String.Format("{0}.{1}.{2}", from.EndDateTime.Year, from.EndDateTime.Month, from.EndDateTime.Day),
                               HtmlPath = from.HtmlPath,
                               NewsletterId = from.Id,
                               PicturePath = from.PicturePath, 
                               Title = from.Title
                           };
            }
            catch { return new CompanyGroup.Dto.WebshopModule.Newsletter(); }
        }
    }
}
