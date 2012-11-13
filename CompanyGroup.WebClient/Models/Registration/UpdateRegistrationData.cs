using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class UpdateRegistrationData : CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData
    {
        public UpdateRegistrationData(CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData updateRegistrationData)
        {
            this.Message = updateRegistrationData.Message;

            this.Successed = updateRegistrationData.Successed;
        }
    }
}