using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    /// <summary>
    /// képek szolgáltatás
    /// </summary>
    public class PictureService : IPictureService
    {
        private const string CACHEKEY_PICTURE = "picture";

        /// <summary>
        /// struktúra cache időtartama percben
        /// </summary>
        private const double CACHE_EXPIRATION_PICTURE = 2880d;

        private static readonly bool PictureCacheEnabled = Helpers.ConfigSettingsParser.GetBoolean("PictureCacheEnabled", true);

        private CompanyGroup.Domain.WebshopModule.IPictureRepository pictureRepository;

        public PictureService(CompanyGroup.Domain.WebshopModule.IPictureRepository pictureRepository)
        {
            if (pictureRepository == null)
            {
                throw new ArgumentNullException("PictureRepository");
            }

            this.pictureRepository = pictureRepository;
        }

        /// <summary>
        /// képek listát visszaadó service
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Pictures GetListByProduct(CompanyGroup.Dto.WebshopModule.PictureFilterRequest request) 
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "The GetListByProduct request parameter can not be null!");

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(request.ProductId), "The GetListByProduct request parameter can not be null!");

                List<CompanyGroup.Domain.WebshopModule.Picture> pictureList = pictureRepository.GetListByProduct(request.ProductId);

                CompanyGroup.Helpers.DesignByContract.Ensure((pictureList != null), "Repository GetListByProduct result cannot be null!");

                if (pictureList.Count == 0) { return new CompanyGroup.Dto.WebshopModule.Pictures(); }

                CompanyGroup.Domain.WebshopModule.Pictures pictures = new CompanyGroup.Domain.WebshopModule.Pictures();

                pictures.AddRange(pictureList);

                return new PicturesToPictures().Map(pictures);
            }
            catch(Exception ex) 
            { 
                throw ex; 
            }
        }

        /// <summary>
        /// elsődleges kép lekérdezése
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public Stream GetPrimaryPicture(string productId, string maxWidth, string maxHeight)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(productId), "The GetPrimaryPicture productId parameter can not be null!");

            try
            {
                CompanyGroup.Domain.WebshopModule.Picture picture = pictureRepository.GetPrimaryPicture(productId);

                CompanyGroup.Helpers.DesignByContract.Ensure((picture != null), "Repository GetPrimaryPicture result cannot be null!");

                byte[] buffer = this.ReadCachedFileContent(picture);

                int w = 0;
                int h = 0;

                if (!Int32.TryParse(maxWidth, out w)) { w = 0; }
                if (!Int32.TryParse(maxHeight, out h)) { h = 0; }

                return CompanyGroup.Helpers.ImageManager.ReSizeFileStreamImage(new MemoryStream(buffer), w, h, "image/jpeg");               
            }
            catch { return null; }
        }

        /// <summary>
        /// termékazonosítóhoz és rekordazonosítóhoz tartozó képtartalmat adja vissza 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="recId"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public Stream GetItem(string productId, string recId, string maxWidth, string maxHeight) 
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(productId), "The PictureService GetItem request productId parameter can not be null!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(recId), "The PictureService GetItem request recId parameter can not be null!");

            try
            {
                List<CompanyGroup.Domain.WebshopModule.Picture> pictures = pictureRepository.GetListByProduct(productId);

                CompanyGroup.Helpers.DesignByContract.Ensure((pictures != null), "Repository GetItem result cannot be null!");

                if (pictures.Count == 0) { return null; }

                long recordId = 0;

                CompanyGroup.Domain.WebshopModule.Picture picture;

                if (!Int64.TryParse(recId, out recordId))
                {
                    picture = pictures.FirstOrDefault();
                }
                else
                {
                    picture = pictures.FirstOrDefault(x => x.RecId.Equals(recordId));
                }

                if (picture == null) { return null; }

                byte[] buffer = this.ReadCachedFileContent(picture);

                int w = 0;
                int h = 0;

                if (!Int32.TryParse(maxWidth, out w)) { w = 0; }
                if (!Int32.TryParse(maxHeight, out h)) { h = 0; }

                return CompanyGroup.Helpers.ImageManager.ReSizeFileStreamImage(new MemoryStream(buffer), w, h, "image/jpeg");
            }
            catch { return null; }
        }

        /// <summary>
        /// képtartalom kiolvasása képazonosító alapján
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public Stream GetItemById(int pictureId, string maxWidth, string maxHeight) 
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((pictureId > 0), "The PictureService GetItemById request pictureId parameter can not be null!");

                CompanyGroup.Domain.WebshopModule.Picture picture = pictureRepository.GetItemById(pictureId);

                if (picture == null) { return null; }

                byte[] buffer = this.ReadCachedFileContent(picture);

                int w = 0;
                int h = 0;

                if (!Int32.TryParse(maxWidth, out w)) { w = 0; }
                if (!Int32.TryParse(maxHeight, out h)) { h = 0; }

                return CompanyGroup.Helpers.ImageManager.ReSizeFileStreamImage(new MemoryStream(buffer), w, h, "image/jpeg");
            }
            catch { return null; }
        }

        /// <summary>
        /// képtartalom kiolvasása számla elem rekordazonosító alapján
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public Stream GetInvoicePicture(int id, string maxWidth, string maxHeight)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((id > 0), "The PictureService GetInvoicePicture request recId parameter can not be null!");

                CompanyGroup.Domain.WebshopModule.Picture picture = pictureRepository.GetInvoicePicture(id);

                if (picture == null) { return null; }

                byte[] buffer = this.ReadCachedFileContent(picture);

                int w = 0;
                int h = 0;

                if (!Int32.TryParse(maxWidth, out w)) { w = 0; }
                if (!Int32.TryParse(maxHeight, out h)) { h = 0; }

                return CompanyGroup.Helpers.ImageManager.ReSizeFileStreamImage(new MemoryStream(buffer), w, h, "image/jpeg");
            }
            catch { return null; }
        }

        /// <summary>
        /// képtartalom kiolvasása megrendelés elem rekordazonosító alapján
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public Stream GetSalesOrderPicture(int id, string maxWidth, string maxHeight)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((id > 0), "The PictureService GetInvoicePicture request Id parameter can not be null!");

                CompanyGroup.Domain.WebshopModule.Picture picture = pictureRepository.GetSalesOrderPicture(id);

                if (picture == null) { return null; }

                byte[] buffer = this.ReadFileContent(picture);

                int w = 0;
                int h = 0;

                if (!Int32.TryParse(maxWidth, out w)) { w = 0; }
                if (!Int32.TryParse(maxHeight, out h)) { h = 0; }

                return CompanyGroup.Helpers.ImageManager.ReSizeFileStreamImage(new MemoryStream(buffer), w, h, "image/jpeg");
            }
            catch { return null; }
        }

        /// <summary>
        /// cache-elve kiolvassa byte tömbben a képet 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        private byte[] ReadCachedFileContent(CompanyGroup.Domain.WebshopModule.Picture picture)
        {
            byte[] buffer = null;

            try
            {
                string cacheKey = String.Empty;

                //cache kiolvasás, ha engedélyezett a kiolvasás
                if (PictureService.PictureCacheEnabled)
                {
                    string itemKey = String.Format("{0}_{1}", picture.Id, picture.RecId);

                    cacheKey = CompanyGroup.Helpers.ContextKeyManager.CreateKey(CACHEKEY_PICTURE, itemKey);

                    buffer = CompanyGroup.Helpers.CacheHelper.Get<byte[]>(cacheKey);
                }

                //ha nem sikerült a cache kiolvasása, akkor olvasás file-ból
                if (buffer == null)
                {
                    buffer = this.ReadFileContent(picture);

                    //cache-be mentés
                    if ((PictureService.PictureCacheEnabled) && (buffer != null))
                    {
                        CompanyGroup.Helpers.CacheHelper.Add<byte[]>(cacheKey, buffer, DateTime.Now.AddMinutes(CompanyGroup.Helpers.CacheHelper.CalculateAbsExpirationInMinutes(CACHE_EXPIRATION_PICTURE)));
                    }
                }

                return buffer;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// file megosztáson lévő képet felolvassa byte tömbbe, cache használat nélkül
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        private byte[] ReadFileContent(CompanyGroup.Domain.WebshopModule.Picture picture)
        {
            byte[] buffer = null;

            FileStream fileStream = null;

            try
            {
                string path = Path.Combine(CompanyGroup.Helpers.ConfigSettingsParser.GetString("PicturePhysicalPath", @"\\axos3\ProductPictures"), picture.FileName);

                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                int length = (int) fileStream.Length;  

                buffer = new byte[length];            

                int count;                            

                int sum = 0;                          

                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                {
                    sum += count;  
                }

                return buffer;
            }
            catch
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                return null;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
    }
}
