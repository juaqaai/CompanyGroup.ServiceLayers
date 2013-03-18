using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IPictureRepository
    {
        /// <summary>
        /// képek lista termékazonosító szerint 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        List<CompanyGroup.Domain.WebshopModule.Picture> GetListByProduct(string productId);

        /// <summary>
        /// kép azonosító szerint
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.Picture GetItemById(int pictureId);

        /// <summary>
        /// számlasorhoz tartozó termékkép kiolvasás
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyGroup.Domain.WebshopModule.Picture GetInvoicePicture(int id);
    }
}
