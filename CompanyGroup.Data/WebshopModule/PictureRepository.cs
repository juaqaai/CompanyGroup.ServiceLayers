using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.WebshopModule
{
    public class PictureRepository : CompanyGroup.Data.NoSql.Repository<CompanyGroup.Domain.WebshopModule.Product>, CompanyGroup.Domain.WebshopModule.IPictureRepository
    {
        public PictureRepository(CompanyGroup.Data.NoSql.ISettings settings) : base(settings) { }

        /// <summary>
        /// képek lista termékazonosító és vállalatkód szerint 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.Picture> GetListByProduct(string productId, string dataAreaId)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(productId), "The productId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(dataAreaId), "The dataAreaId parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.WebshopModule.Product> collection = this.GetCollection();

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("ProductId", productId),
                                                                      MongoDB.Driver.Builders.Query.EQ("DataAreaId", dataAreaId));

                CompanyGroup.Domain.WebshopModule.Product product = collection.FindOne(query);

                if (product == null) return new List<Domain.WebshopModule.Picture>();

                List<CompanyGroup.Domain.WebshopModule.Picture> pictures = product.Pictures.ConvertAll(x => CompanyGroup.Domain.WebshopModule.Factory.CreatePicture(x.FileName, x.Primary, x.RecId));

                return pictures;
            }
            catch
            {
                return new List<CompanyGroup.Domain.WebshopModule.Picture>();
            }
            finally
            {
                Disconnect();
            }            
        }

        /// <summary>
        /// képek lista elsődleges kulcs szerint
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.Picture> GetListByKey(string objectId)
        {
            try
            {
                this.ReConnect();

                CompanyGroup.Domain.Utils.Check.Require(!String.IsNullOrWhiteSpace(objectId), "The _id parameter cannot be null!");

                MongoCollection<CompanyGroup.Domain.WebshopModule.Product> collection = this.GetCollection();

                IMongoQuery query = MongoDB.Driver.Builders.Query.And(MongoDB.Driver.Builders.Query.EQ("_id", objectId));

                CompanyGroup.Domain.WebshopModule.Product product = collection.FindOne(query);

                List<CompanyGroup.Domain.WebshopModule.Picture> pictures = product.Pictures.ConvertAll(x => CompanyGroup.Domain.WebshopModule.Factory.CreatePicture(x.FileName, x.Primary, x.RecId));

                return pictures;
            }
            catch
            {
                return new List<CompanyGroup.Domain.WebshopModule.Picture>();
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
