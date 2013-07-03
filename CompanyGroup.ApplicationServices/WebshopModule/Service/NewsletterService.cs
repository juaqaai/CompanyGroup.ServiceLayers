using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// hírlevél szolgáltatás
    /// </summary>
    public class NewsletterService : ServiceCoreBase, INewsletterService
    {
        private CompanyGroup.Domain.WebshopModule.INewsletterRepository newsletterRepository;

        public NewsletterService(CompanyGroup.Domain.WebshopModule.INewsletterRepository newsletterRepository, 
                                 CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (newsletterRepository == null)
            {
                throw new ArgumentNullException("NewsletterRepository");
            }

            this.newsletterRepository = newsletterRepository;
        }

        /// <summary>
        /// hírlevél gyűjetemény lekérdezése 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.NewsletterCollection GetNewsletterCollection(CompanyGroup.Dto.WebshopModule.GetNewsletterCollectionRequest request)
        {
            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                //látogató lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Helpers.DesignByContract.Ensure(visitor != null, "Visitor can not be null!");

                CompanyGroup.Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "Visitor must be logged in!");

                List<CompanyGroup.Domain.WebshopModule.Newsletter> newsletters = new List<CompanyGroup.Domain.WebshopModule.Newsletter>();

                //ha regisztrált mindkét vállalatban, akkor mindkét vállalat listáját megkapja
                //ha nem, akkor csak annak a vállalatnak a listáját, amelyikben regisztrálva van
                visitor.AuthorizedDataAreaList().ForEach(x =>
                {
                    List<CompanyGroup.Domain.WebshopModule.Newsletter> tmp = newsletterRepository.GetNewsletterList(CompanyGroup.Helpers.ConfigSettingsParser.GetInt("NewsletterListLimit", 100), x, String.Empty, request.ManufacturerId);

                    newsletters.AddRange(tmp);
                });

                CompanyGroup.Domain.WebshopModule.NewsletterCollection newsletterCollection = new CompanyGroup.Domain.WebshopModule.NewsletterCollection(newsletters);

                List<CompanyGroup.Dto.WebshopModule.Newsletter> items = newsletterCollection.Newsletters.ConvertAll(x => new NewsletterToNewsletter().Map(x));

                //válasz elkészítése
                CompanyGroup.Dto.WebshopModule.NewsletterCollection result = new CompanyGroup.Dto.WebshopModule.NewsletterCollection(items, new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor));

                return result;
            }
            catch (Exception ex)
            { 
                throw(ex);
            }
        }

        /// <summary>
        /// hírlevél lista hírlevél azonosító paraméterek alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.NewsletterCollection GetNewsletterListByFilter(CompanyGroup.Dto.WebshopModule.GetNewsletterListByFilterRequest request)
        {
            try
            {

                Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

                //látogató lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Helpers.DesignByContract.Ensure(visitor != null, "Visitor can not be null!");

                CompanyGroup.Helpers.DesignByContract.Ensure(visitor.IsValidLogin, "Visitor must be logged in!");

                request.NewsletterIdList.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                List<CompanyGroup.Domain.WebshopModule.Newsletter> newsletters = newsletterRepository.GetNewsletterListByFilter(CompanyGroup.Helpers.ConfigSettingsParser.GetInt("NewsletterListLimit", 0), String.Empty, CompanyGroup.Helpers.ConvertData.ConvertStringListToDelimitedString(request.NewsletterIdList));

                CompanyGroup.Domain.WebshopModule.NewsletterCollection newsletterCollection = new CompanyGroup.Domain.WebshopModule.NewsletterCollection(newsletters);

                List<CompanyGroup.Dto.WebshopModule.Newsletter> items = newsletterCollection.Newsletters.ConvertAll(x => new NewsletterToNewsletter().Map(x));

                //válasz elkészítése
                CompanyGroup.Dto.WebshopModule.NewsletterCollection result = new CompanyGroup.Dto.WebshopModule.NewsletterCollection(items, new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor));

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }


}
