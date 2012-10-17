using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Adapter
{
    public class PictureToPicture
    {
        /// <summary>
        /// dto picture -> dto picture
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public CompanyGroup.GlobalServices.Dto.Picture Map(CompanyGroup.Dto.WebshopModule.Picture picture)
        {
            return new CompanyGroup.GlobalServices.Dto.Picture() { FileName = picture.FileName, Id = picture.RecId };
        }
    }
}
