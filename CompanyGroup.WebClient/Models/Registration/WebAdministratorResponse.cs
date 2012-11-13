using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class WebAdministratorResponse : CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator
    {
        public WebAdministratorResponse(CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator webAdministrator)
        {
            this.Message = webAdministrator.Message;

            this.Successed = webAdministrator.Successed;
        }
    }
}