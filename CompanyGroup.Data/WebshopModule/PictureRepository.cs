﻿using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.WebshopModule
{
    public class PictureRepository : CompanyGroup.Domain.WebshopModule.IPictureRepository
    {
        /// <summary>
        /// konstruktor
        /// </summary>
        public PictureRepository() { }

        /// <summary>
        /// nhibernate web session
        /// </summary>
        private NHibernate.ISession Session
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession(); }
        }

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

        /// <summary>
        /// termékhez tartozó elsődleges kép
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></return>
        public CompanyGroup.Domain.WebshopModule.Picture GetPrimaryPicture(string productId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(productId), "The productId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.PictureSelect")
                                                 .SetString("ProductId", productId)
                                                 .SetResultTransformer(
                                                  new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Picture).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.Picture> pictures = query.List<CompanyGroup.Domain.WebshopModule.Picture>() as List<CompanyGroup.Domain.WebshopModule.Picture>;

                if (pictures == null) return new CompanyGroup.Domain.WebshopModule.Picture();

                CompanyGroup.Domain.WebshopModule.Picture picture = pictures.Find(x => x.Primary);

                return (picture == null) ? new CompanyGroup.Domain.WebshopModule.Picture() : picture;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Picture();
            }
        }

        public CompanyGroup.Domain.WebshopModule.Picture GetItemById(int pictureId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((pictureId > 0), "The PictureId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.PictureItemSelect")
                                                .SetInt32("PictureId", pictureId)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Picture).GetConstructors()[0]));

                CompanyGroup.Domain.WebshopModule.Picture picture = query.UniqueResult<CompanyGroup.Domain.WebshopModule.Picture>();

                if (picture == null) return new Domain.WebshopModule.Picture();

                return picture;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Picture();
            }
        }

        //[InternetUser].[InvoicePictureSelect]( @RecId BIGINT = 0 )	
        public CompanyGroup.Domain.WebshopModule.Picture GetInvoicePicture(int id)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((id > 0), "The PictureId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.InvoicePictureSelect")
                                                .SetInt32("Id", id)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Picture).GetConstructors()[0]));

                CompanyGroup.Domain.WebshopModule.Picture picture = query.UniqueResult<CompanyGroup.Domain.WebshopModule.Picture>();

                if (picture == null) return new Domain.WebshopModule.Picture();

                return picture;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Picture();
            }
        }

        public CompanyGroup.Domain.WebshopModule.Picture GetSalesOrderPicture(int id)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((id > 0), "The PictureId parameter cannot be null!");

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.SalesOrderPictureSelect")
                                                .SetInt32("Id", id)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Picture).GetConstructors()[0]));

                CompanyGroup.Domain.WebshopModule.Picture picture = query.UniqueResult<CompanyGroup.Domain.WebshopModule.Picture>();

                if (picture == null) return new Domain.WebshopModule.Picture();

                return picture;
            }
            catch
            {
                return new CompanyGroup.Domain.WebshopModule.Picture();
            }
        }

    }
}
