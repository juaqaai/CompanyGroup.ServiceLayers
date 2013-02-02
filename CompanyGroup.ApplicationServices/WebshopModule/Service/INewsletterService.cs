using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public interface INewsletterService
    {
        /// <summary>
        /// hírlevél gyűjetemény lekérdezése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.WebshopModule.NewsletterCollection GetNewsletterCollection(CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest request);
    }
}
