using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.WebshopModule
{
    public class PictureRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.WebshopModule.IPictureRepository
    {
        public PictureRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// képek lista termékazonosító és vállalatkód szerint 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.Picture> GetListByProduct(string productId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(productId), "The productId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.PictureSelect")
                                                .SetString("ProductId", productId)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Picture).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.Picture> pictures = query.List<CompanyGroup.Domain.WebshopModule.Picture>() as List<CompanyGroup.Domain.WebshopModule.Picture>;

                if (pictures == null) return new List<Domain.WebshopModule.Picture>();

                return pictures;
            }
            catch
            {
                return new List<CompanyGroup.Domain.WebshopModule.Picture>();
            }
        }

    }
}
