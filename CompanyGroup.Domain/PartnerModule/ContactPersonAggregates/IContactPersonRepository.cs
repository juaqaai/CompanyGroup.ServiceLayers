using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public interface IContactPersonRepository
    {
        /// <summary>
        /// jelszómódosítás ellenörzése
        /// </summary>
        /// <param name="contactPersonId"></param>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        CompanyGroup.Domain.PartnerModule.ChangePasswordVerify VerifyChangePassword(string contactPersonId, string userName, string oldPassword, string newPassword, string dataAreaId);


    }
}
