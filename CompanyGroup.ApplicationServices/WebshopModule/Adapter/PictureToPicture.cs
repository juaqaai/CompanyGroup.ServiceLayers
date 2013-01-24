using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class PictureToPicture
    {
        /// <summary>
        /// domain picture -> dto picture
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.Picture Map(CompanyGroup.Domain.WebshopModule.Picture picture)
        {
            return new CompanyGroup.Dto.WebshopModule.Picture(picture.FileName, picture.Primary, picture.RecId, picture.Id);
        }
    }
}
