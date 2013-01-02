using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                InstanceContextMode = InstanceContextMode.PerCall,
    //                ConcurrencyMode = ConcurrencyMode.Multiple,
    //                IncludeExceptionDetailInFaults = true),
    //                System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()]
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
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Pictures GetListByProduct(CompanyGroup.Dto.ServiceRequest.PictureFilter request) 
        {
            try
            {
                List<CompanyGroup.Domain.WebshopModule.Picture> pictureList = pictureRepository.GetListByProduct(request.ProductId);

                if (pictureList.Count == 0) { return new CompanyGroup.Dto.WebshopModule.Pictures(); }

                CompanyGroup.Domain.WebshopModule.Pictures pictures = new CompanyGroup.Domain.WebshopModule.Pictures();

                pictures.AddRange(pictureList);

                return new PicturesToPictures().Map(pictures);
            }
            catch { return new CompanyGroup.Dto.WebshopModule.Pictures(); }
        }

        /// <summary>
        /// képtartalmat adja vissza 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="recId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Stream GetItem(string productId, string recId, string maxWidth, string maxHeight) //CompanyGroup.Dto.ServiceRequest.PictureFilter request
        {
            try
            {
                List<CompanyGroup.Domain.WebshopModule.Picture> pictures = pictureRepository.GetListByProduct(productId);

                if (pictures.Count == 0) { return null; }

                CompanyGroup.Domain.WebshopModule.Picture picture = pictures.FirstOrDefault(x => x.RecId.Equals(Convert.ToInt64(recId)));

                if (picture == null) { return null; }

                //System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";

                FileStream fileStream = ReadFileContent(picture);
                //string path = Path.Combine(CompanyGroup.Helpers.ConfigSettingsParser.GetString("PicturePhysicalPath", @"\\axos3\ProductPictures"), picture.FileName);

                int w = 0;
                int h = 0;

                if (!Int32.TryParse(maxWidth, out w)) { w = 0; }
                if (!Int32.TryParse(maxHeight, out h)) { h = 0; }

                //System.Drawing.Bitmap imgToResize = new System.Drawing.Bitmap(fileStream);

                // PictureService.ResizeImage(imgToResize, new System.Drawing.Size(w, h)); //ResizeImage(fileStream, w, h);

                return CompanyGroup.Helpers.ImageManager.ReSizeFileStreamImage(fileStream, w, h, "image/jpeg");
            }
            catch { return null; }
        }

        /// <summary>
        /// képtartalom képazonosító alapján
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public Stream GetItemById(int pictureId, string maxWidth, string maxHeight) 
        {
            try
            {
                CompanyGroup.Domain.WebshopModule.Picture picture = pictureRepository.GetItemById(pictureId);

                if (picture == null) { return null; }

                FileStream fileStream = ReadFileContent(picture);

                int w = 0;
                int h = 0;

                if (!Int32.TryParse(maxWidth, out w)) { w = 0; }
                if (!Int32.TryParse(maxHeight, out h)) { h = 0; }

                return CompanyGroup.Helpers.ImageManager.ReSizeFileStreamImage(fileStream, w, h, "image/jpeg");
            }
            catch { return null; }
        }

        private FileStream ReadFileContent(CompanyGroup.Domain.WebshopModule.Picture picture)
        {
            FileStream fileStream = null;
            try
            {
                string path = Path.Combine(CompanyGroup.Helpers.ConfigSettingsParser.GetString("PicturePhysicalPath", @"\\axos3\ProductPictures"), picture.FileName);

                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                return fileStream;
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
                    //fileStream.Close();
                }
            }
        }

        private Stream ResizeImage(FileStream fileStream, int width, int height)
        {
            if (fileStream == null) { return null; }

            if (width == 0) { width = 200; }

            if (height == 0) { width = 200; }

            string settingsForImages = String.Format("maxwidth={0}&maxheight={1}", width, height);   //"width={0}&height={1}&format=jpg&crop=auto" ,    maxwidth=100;maxheight=100

            ImageResizer.ResizeSettings resizeSettings = new ImageResizer.ResizeSettings(settingsForImages);

            //using (MemoryStream ms = new MemoryStream()) 
            //{
            MemoryStream ms = new MemoryStream();

                ImageResizer.ImageBuilder.Current.Build(fileStream, ms, resizeSettings);

                return (Stream) ms;
            //}
        }

        private static Stream ResizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            System.Drawing.Bitmap b = new System.Drawing.Bitmap(destWidth, destHeight);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)b);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);

            g.Dispose();

            return SaveBitmapToStream(b, "image/jpeg", 100L);
        }

        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            System.Drawing.Imaging.ImageCodecInfo info = null;

            string s = !String.IsNullOrEmpty(mimeType) ? mimeType : "image/jpeg";

            var encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (int j = 0; (j < encoders.Length); ++j)
            {
                if (encoders[j].MimeType.Equals(s))
                {
                    info = encoders[j];
                }
            }
            return info;
        }

        private static Stream SaveBitmapToStream(System.Drawing.Bitmap bmp, string mimeType, long quality)
        {
            System.Drawing.Imaging.EncoderParameters eps = new System.Drawing.Imaging.EncoderParameters(1);

            eps.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo(mimeType);    //"image/jpeg"

            MemoryStream ms = new MemoryStream();

            bmp.Save(ms, ici, eps);

            return ms;
        }
    }
}
