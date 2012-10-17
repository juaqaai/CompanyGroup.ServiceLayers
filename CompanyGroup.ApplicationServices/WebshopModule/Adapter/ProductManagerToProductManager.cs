using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public class ProductManagerToProductManager
    {
        public CompanyGroup.Dto.PartnerModule.ProductManager Map(CompanyGroup.Domain.PartnerModule.ProductManager productManager)
        {
            try
            {
                return new CompanyGroup.Dto.PartnerModule.ProductManager()
                       {
                           Email = productManager.Email,
                           Extension = productManager.Extension,
                           Mobile = productManager.Mobile,
                           Name = productManager.Name
                       };
            }
            catch { return new CompanyGroup.Dto.PartnerModule.ProductManager(); }
        }
    }
}
