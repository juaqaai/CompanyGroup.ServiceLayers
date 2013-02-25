using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class PicturesToPictures
    {
        /// <summary>
        /// Domain pictures -> DTO pictures
        /// </summary>
        /// <param name="pictures"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Pictures Map(List<CompanyGroup.Domain.WebshopModule.Picture> pictures)
        {
            try
            {
                CompanyGroup.Dto.WebshopModule.Pictures result = new CompanyGroup.Dto.WebshopModule.Pictures();

                result.Items.AddRange(pictures.ConvertAll(x => new PictureToPicture().Map(x)));

                return result;
            }
            catch { return new CompanyGroup.Dto.WebshopModule.Pictures(); }
        }
    }
}
