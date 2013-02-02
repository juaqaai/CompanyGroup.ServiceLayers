using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public interface IPictureService
    {

        System.IO.Stream GetItem(string productId, string recId, string width, string height); 

        System.IO.Stream GetItemById(int pictureId, string maxWidth, string maxHeight);

        CompanyGroup.Dto.WebshopModule.Pictures GetListByProduct(CompanyGroup.Dto.WebshopModule.PictureFilterRequest request);
    }
}
