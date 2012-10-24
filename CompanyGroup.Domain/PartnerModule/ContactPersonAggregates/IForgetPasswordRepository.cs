using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IForgetPasswordRepository
    {
        /// <summary>
        /// elfelejtett jelszó kiolvasása kulcs alapján
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.PartnerModule.ForgetPassword GetItemByKey(string id);

        /// <summary>
        /// elfelejtett jelszó új bejegyzés hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        void Add(CompanyGroup.Domain.PartnerModule.ForgetPassword forgetPassword);

        /// <summary>
        /// elfelejtett jelszó státusz állapot beállítása 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataAreaId"></param>
        void SetStatus(string id, CompanyGroup.Domain.PartnerModule.ForgetPasswordStatus status);

        /// <summary>
        /// elfelejtett jelszó művelet (AX)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Domain.PartnerModule.ForgetPasswordCreateResult Forget(CompanyGroup.Domain.PartnerModule.ForgetPasswordCreate request);

    }
}
