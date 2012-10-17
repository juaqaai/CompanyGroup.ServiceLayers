using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices
{
    /// <summary>
    /// ősosztály a visitor service-ek összefogására
    /// </summary>
    public class ServiceCoreBase
    {
        protected CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository;

        private const string CACHEKEY_VISITOR = "visitor";

        /// <summary>
        /// 30 percig cache-be kerül a visitor objektum
        /// </summary>
        private const double CACHE_EXPIRATION_VISITOR = 30d;

        /// <summary>
        /// konstruktor visitor repository-val
        /// </summary>
        /// <param name="customerRepository"></param>
        public ServiceCoreBase(CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository)
        {
            if (visitorRepository == null)
            {
                throw new ArgumentNullException("VisitorRepository");
            }

            this.visitorRepository = visitorRepository;
        }

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó mentett információk kiolvasása
        /// - ha a kulcs üres, akkor új visitor példánnyal tér vissza
        /// - ha nincs a cache-ben az objektum, akkor repository hívás történik
        /// - ha nem érvényes a bejelentkezés, akkor repository hívás történik
        /// - egyebkent visszaadásra kerül a cache tartalma
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected CompanyGroup.Domain.PartnerModule.Visitor GetVisitor(string objectId)
        {
            if (String.IsNullOrWhiteSpace(objectId))
            {
                return CompanyGroup.Domain.PartnerModule.Factory.CreateVisitor();
            }

            CompanyGroup.Domain.PartnerModule.Visitor visitor = CompanyGroup.Helpers.CacheHelper.Get<CompanyGroup.Domain.PartnerModule.Visitor>(CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_VISITOR, objectId));

            if ((visitor == null) || (!visitor.IsValidLogin))
            {
                visitor = this.GetVisitorFromRepository(objectId);
            }

            return visitor;
        }

        /// <summary>
        /// visitor kikeresése repository-ból, majd cache-be mentés
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        private CompanyGroup.Domain.PartnerModule.Visitor GetVisitorFromRepository(string objectId)
        {
            try
            {
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemByKey(objectId);

                visitor.SetLoggedIn();

                //cache-be mentés csak akkor, ha érvényes a login
                if (visitor.IsValidLogin)
                {
                    visitor.Representative.SetDefault();

                    CompanyGroup.Helpers.CacheHelper.Add<CompanyGroup.Domain.PartnerModule.Visitor>(CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_VISITOR, objectId), visitor, DateTime.Now.AddMinutes(CACHE_EXPIRATION_VISITOR));
                }

                return visitor;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                visitorRepository.Disconnect();
            }
        }

        /// <summary>
        /// levél txt template
        /// </summary>
        /// <param name="mailTextTemplateFile"></param>
        protected static string PlainText(string mailTextTemplateFile)
        {
            System.IO.StreamReader sr = null;
            try
            {
                string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + mailTextTemplateFile);

                sr = new System.IO.StreamReader(filepath);

                return sr.ReadToEnd();
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (sr != null) { sr.Close(); }
            }
        }

        /// <summary>
        /// finanszírozási ajánlat levél html template
        /// </summary>
        protected static string HtmlText(string mailTextTemplateFile)
        {
            System.IO.StreamReader sr = null;
            try
            {
                string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + mailTextTemplateFile);

                sr = new System.IO.StreamReader(filepath);

                return sr.ReadToEnd();
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (sr != null) { sr.Close(); }
            }
        }

    }
}
