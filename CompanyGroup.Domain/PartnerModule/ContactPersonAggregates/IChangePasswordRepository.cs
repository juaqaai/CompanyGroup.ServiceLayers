using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IChangePasswordRepository
    {
                /// <summary>
        /// jelszómódosítás kiolvasása kulcs alapján
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.PartnerModule.ChangePassword GetItemByKey(string id);

                /// <summary>
        /// jelszómódosítás új bejegyzés hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        void Add(CompanyGroup.Domain.PartnerModule.ChangePassword changePassword);

                /// <summary>
        /// jelszómódosítás státusz állapot beállítása 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataAreaId"></param>
        void SetStatus(string id, CompanyGroup.Domain.PartnerModule.ChangePasswordStatus status);

        /// <summary>
        /// jelszómódosítás művelete
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Domain.PartnerModule.ChangePasswordCreateResult Change(CompanyGroup.Domain.PartnerModule.ChangePasswordCreate request);

    }
}
