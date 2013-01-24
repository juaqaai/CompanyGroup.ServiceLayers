using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    public interface IFinanceService
    {
        CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment CreateFinanceOffer(CompanyGroup.Dto.WebshopModule.CreateFinanceOfferRequest request);
    }
}
