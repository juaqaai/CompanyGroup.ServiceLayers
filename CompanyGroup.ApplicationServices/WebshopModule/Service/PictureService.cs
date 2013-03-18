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
            CompanyGroup.Helpers.DesignByContract.Require((request != null), "The GetListByProduct request parameter can not be null!");

            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(request.ProductId), "The GetListByProduct request parameter can not be null!");

            try
            {
                List<CompanyGroup.Domain.WebshopModule.Picture> pictureList = pictureRepository.GetListByProduct(request.ProductId);

                CompanyGroup.Helpers.DesignByContract.Ensure((pictureList != null), "Repository GetListByProduct result cannot be null!");

                if (pictureList.Count == 0) { return new CompanyGroup.Dto.WebshopModule.Pictures(); }

                CompanyGroup.Domain.WebshopModule.Pictures pictures = new CompanyGroup.Domain.WebshopModule.Pictures();

                pictures.AddRange(pictureList);

                return new PicturesToPictures().Map(pictures);
            }
            catch { return new CompanyGroup.Dto.WebshopModule.Pictures(); }
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

                byte[] buffer = this.ReadFileContent(picture);

                int w = 0;
                int h = 0;

                if (!Int32.TryParse(maxWidth, out w)) { w = 0; }
                if (!Int32.TryParse(maxHeight, out h)) { h = 0; }

                return CompanyGroup.Helpers.ImageManager.ReSizeFileStreamImage(new MemoryStream(buffer), w, h, "image/jpeg");
            }
            catch { return null; }
        }

        private byte[] ReadFileContent(CompanyGroup.Domain.WebshopModule.Picture picture)
        {
            byte[] buffer;

            FileStream fileStream = null;

            try
            {
                string path = Path.Combine(CompanyGroup.Helpers.ConfigSettingsParser.GetString("PicturePhysicalPath", @"\\axos3\ProductPictures"), picture.FileName);

                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                int length = (int) fileStream.Length;  // get file length

                buffer = new byte[length];            // create buffer

                int count;                            // actual number of bytes read

                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                {
                    sum += count;  // sum is a buffer offset for next reading
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
