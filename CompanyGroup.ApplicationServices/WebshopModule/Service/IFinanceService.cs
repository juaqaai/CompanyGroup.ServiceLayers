using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    [ServiceContract(Namespace = "http://CompanyGroup.ApplicationServices.WebshopModule/", Name = "FinanceService")]
    public interface IFinanceService
    {
        CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment CreateFinanceOffer(CompanyGroup.Dto.ServiceRequest.CreateFinanceOffer request);
    }
}
