using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// regisztrációs adatok módosítása válaszüzenet
    /// </summary>
    public class UpdateRegistrationData : CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData
    {
        public UpdateRegistrationData(CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData updateRegistrationData)
        {
            this.Message = updateRegistrationData.Message;

            this.Successed = updateRegistrationData.Successed;

            this.Visitor = updateRegistrationData.Visitor;
        }
    }
}