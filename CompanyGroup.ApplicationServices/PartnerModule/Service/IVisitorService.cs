using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public interface IVisitorService
    {
        /// <summary>
        /// bejelentkezés           
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.PartnerModule.Visitor SignIn(CompanyGroup.Dto.PartnerModule.SignInRequest request);

        /// <summary>
        /// kijelentkezés   
        /// </summary>
        /// <param name="request"></param>
        void SignOut(CompanyGroup.Dto.PartnerModule.SignOutRequest request);

        /// <summary>
        /// bejelentkezett látogatóhoz kapcsolódó mentett információk kiolvasása            
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.PartnerModule.Visitor GetVisitorInfo(CompanyGroup.Dto.PartnerModule.VisitorInfoRequest request);

        /// <summary>
        /// valutanem csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.PartnerModule.Visitor ChangeCurrency(CompanyGroup.Dto.PartnerModule.ChangeCurrencyRequest request);

        /// <summary>
        /// nyelvválasztás csere
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void ChangeLanguage(CompanyGroup.Dto.PartnerModule.ChangeLanguageRequest request);
    }
}
