using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Adapter
{
    public class PicturesToPictures
    {
        /// <summary>
        /// Domain pictures -> DTO pictures
        /// </summary>
        /// <param name="pictures"></param>
        /// <returns></returns>
        public CompanyGroup.GlobalServices.Dto.Pictures Map(CompanyGroup.Dto.WebshopModule.Pictures pictures)
        {
            try
            {
                CompanyGroup.GlobalServices.Dto.Pictures result = new CompanyGroup.GlobalServices.Dto.Pictures();

                result.Items = new List<CompanyGroup.GlobalServices.Dto.Picture>();

                result.Items.AddRange(pictures.Items.ConvertAll(x => new PictureToPicture().Map(x)));

                return result;
            }
            catch { return new CompanyGroup.GlobalServices.Dto.Pictures(); }
        }
    }
}
