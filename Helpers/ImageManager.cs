namespace CompanyGroup.Helpers
{
    using System;
    using System.IO;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;

    /// <summary>
    /// kép manipuláló segédosztály
    /// </summary>
    public class ImageManager
    {
        /// <summary>
        /// konstruktor
        /// </summary>
        private ImageManager() { }

        #region "kép méretező osztály"

        /// <summary>
        /// beépített kép méretező osztály
        /// </summary>
        private class ImageSize
        {

            /// <summary>
            /// kép szélesség
            /// </summary>
            private int width = 0;

            /// <summary>
            /// kép magasság
            /// </summary>
            private int height = 0;

            /// <summary>
            /// kép szélesség kiolvasása, beállítása
            /// </summary>
            public int Width
            {
                get { return this.width; }
                set { this.width = value; }
            }

            /// <summary>
            /// kép magasság kiolvasása, beállítása
            /// </summary>
            public int Height
            {
                get { return this.height; }
                set { this.height = value; }
            }

            /// <summary>
            /// kép méretarányos átméretezése
            /// </summary>
            /// <param name="iMaxSize"></param>
            /// <returns></returns>
            public void Resize(int iMaxSize)
            {
                if ((this.width > 0) && (this.height > 0))
                {
                    float fMulti = (float)this.width / (float)this.height;
                    if (this.width < this.height)
                    {
                        this.height = iMaxSize;
                        this.width = (int)((float)iMaxSize * fMulti);
                    }
                    else
                    {
                        this.width = iMaxSize;
                        this.height = (int)((float)iMaxSize / fMulti);
                    }
                }
            }


            /// <summary>
            /// kép méretarányos átméretezése szélesség alapján (proportionately)
            /// </summary>
            /// <param name="width"></param>
            /// <returns></returns>
            public void ReSizebyWith(int width)
            {
                if ((this.width > 0) && (this.height > 0) && (width > 0))
                {
                    var fHeight = ((float)this.height * width) / (float)this.width;
                    this.height = (int)fHeight;
                    this.width = width;
                }
            }

            /// <summary>
            /// kép méretarányos átméretezése magasság alapján (proportionately)
            /// </summary>
            /// <param name="height"></param>
            /// <returns></returns>
            public void ReSizebyHeight(int height)
            {
                if ((this.width > 0) && (this.height > 0) && (height > 0))
                {
                    var fWidth = ((float)this.width * height) / (float)this.height;
                    this.width = (int)fWidth;
                    this.height = height;
                }
            }

            /// <summary>
            /// kép méretarányos átméretezése magasság, szélesség alapján (proportionately)
            /// </summary>
            /// <param name="width">width</param>
            /// <param name="height">height</param>
            /// <returns></returns>
            public void DownSize(int width, int height)
            {
                //width downsize condition 
                bool bWidthDownSize = (((this.width - width) > 0) && (width > 0));

                //height downsize condition
                bool bHeightDownSize = (((this.height - height) > 0) && (height > 0));

                if (bWidthDownSize && bHeightDownSize && ((this.width - width) >= (this.height - height)))
                {
                    this.ReSizebyWith(width);
                }
                else if (bWidthDownSize && bHeightDownSize)
                {
                    this.ReSizebyHeight(height);
                }
                else if (bWidthDownSize)
                {
                    this.ReSizebyWith(width);
                }
                else if (bHeightDownSize)
                {
                    this.ReSizebyHeight(height);
                }

            }
        }

        #endregion

        /// <summary>
        /// encoder info kiolvasása 
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo oInfo = null;

            string s = !String.IsNullOrEmpty(mimeType) ? mimeType : "image/jpeg";

            var encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; (j < encoders.Length); ++j)
            {
                if (encoders[j].MimeType.Equals(s))
                {
                    oInfo = encoders[j];
                }
            }
            return oInfo;
        }

        /// <summary>
        /// bitmap elmentése byte tömbbe
        /// </summary>
        /// <param name="oBmp"></param>
        /// <param name="oImageSize"></param>
        /// <param name="mimeType"></param>
        /// <param name="lQuality"></param>
        /// <returns></returns>
        private static byte[] SaveBitmapToByteArray(Bitmap oBmp, ImageSize oImageSize, string mimeType, long lQuality)
        {
            //uj bitmap keszitese
            var newBmp = new Bitmap(oBmp, oImageSize.Width, oImageSize.Height);

            // send the image to the viewer
            EncoderParameters eps = new EncoderParameters(1);
            eps.Param[0] = new EncoderParameter(Encoder.Quality, lQuality);
            ImageCodecInfo ici = GetEncoderInfo(mimeType);    //"image/jpeg"

            //save converted bitmap to memorystream
            MemoryStream msNew = new MemoryStream();
            newBmp.Save(msNew, ici, eps);

            return msNew.ToArray();
        }

        /// <summary>
        /// adatbázis kép méretarányos átméretezése
        /// </summary>
        /// <param name="arrPicture"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static byte[] ReSizeByteArrayImage(byte[] arrPicture, int width, int height, string mimeType)
        {
            return ImageManager.ReSizeByteArrayImage(arrPicture, width, height, mimeType, 100L);
        }

        /// <summary>
        /// adatbázis kép méretarányos átméretezése
        /// </summary>
        /// <param name="arrPicture"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mimeType"></param>
        /// <param name="lQuality"></param>
        /// <returns></returns>
        public static byte[] ReSizeByteArrayImage(byte[] arrPicture, int width, int height, string mimeType, long lQuality)
        {
            if (arrPicture == null)
            {
                return null;
            }

            var ms = new MemoryStream(arrPicture);

            var bm = new Bitmap(ms);

            ImageSize imageSize = new ImageSize() { Width = bm.Width, Height = bm.Height };

            imageSize.DownSize(width, height);

            byte[] arr = ImageManager.SaveBitmapToByteArray(bm, imageSize, mimeType, lQuality);

            bm.Dispose();

            bm = null;

            ms.Close();

            ms = null;

            return arr;
        }

        /// <summary>
        /// fizikai kép méretarányos átméretezése   
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mimeType"></param>
        /// <param name="lQuality"></param>
        /// <returns></returns>
        public static byte[] ReSizePhysicalImage(string fileName, int width, int height, string mimeType, long lQuality)
        {
            Bitmap bm = new Bitmap(fileName);

            ImageSize imageSize = new ImageSize() { Width = bm.Width, Height = bm.Height };

            imageSize.DownSize(width, height);

            byte[] arr = ImageManager.SaveBitmapToByteArray(bm, imageSize, mimeType, lQuality);

            bm.Dispose();

            bm = null;

            return arr;
        }

        /// <summary>
        /// fizikai kép méretarányos átméretezése 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static byte[] ReSizePhysicalImage(string fileName, int width, int height, string mimeType)
        {
            return ReSizePhysicalImage(fileName, width, height, mimeType, 100L);
        }

        public static Stream ReSizeFileStreamImage(Stream stream, int width, int height, string mimeType)
        {
            Bitmap bm = new Bitmap(stream);

            ImageSize imageSize = new ImageSize() { Width = bm.Width, Height = bm.Height };

            imageSize.DownSize(width, height);

            byte[] arr = ImageManager.SaveBitmapToByteArray(bm, imageSize, mimeType, 100L);

            bm.Dispose();

            bm = null;

            return new MemoryStream(arr);
        }

    }
}
