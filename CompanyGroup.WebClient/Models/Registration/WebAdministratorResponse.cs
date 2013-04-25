using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class WebAdministratorResponse
    {
        public WebAdministratorResponse(CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator webAdministrator)
        {
            this.Message = webAdministrator.Message;

            this.Succeeded = webAdministrator.Succeeded;

            this.Visitor = new Visitor(webAdministrator.Visitor);
        }

        /// <summary>
        /// sikeres volt-e a művelet, vagy nem
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// a kérés nyelvétől függő hibaüzenet (ha nincs hiba, akkor üres) 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// látogató adatok
        /// </summary>
        public Visitor Visitor { get; set; }
    }
}