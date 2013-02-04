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
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                //látogató lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                List<CompanyGroup.Domain.WebshopModule.Newsletter> newsletters = new List<CompanyGroup.Domain.WebshopModule.Newsletter>();

                //ha regisztrált mindkét vállalatban, akkor mindkét vállalat listáját megkapja
                //ha nem, akkor csak annak a vállalatnak a listáját, amelyikben regisztrálva van
                visitor.AuthorizedDataAreaList().ForEach(x =>
                {
                    List<CompanyGroup.Domain.WebshopModule.Newsletter> tmp = newsletterRepository.GetNewsletterList(CompanyGroup.Helpers.ConfigSettingsParser.GetInt("NewsletterListLimit", 100), x, String.Empty, request.ManufacturerId);

                    newsletters.AddRange(tmp);
                });

                CompanyGroup.Domain.WebshopModule.NewsletterCollection newsletterCollection = new CompanyGroup.Domain.WebshopModule.NewsletterCollection(newsletters);

                //válasz elkészítése
                CompanyGroup.Dto.WebshopModule.NewsletterCollection result = new CompanyGroup.Dto.WebshopModule.NewsletterCollection();

                result.Items = newsletterCollection.Newsletters.ConvertAll(x => new NewsletterToNewsletter().Map(x));

                return result;
            }
            catch (Exception ex)
            { 
                throw(ex);
            }
        }
    }


}
