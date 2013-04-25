using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    public class UpdateDataRecording
    {
        public string VisitorId { get; set; }

        public string RegistrationId { get; set; }


        public string LanguageId { get; set; }

        /// <summary>
        /// DataRecording
        /// </summary>
        public CompanyGroup.Dto.RegistrationModule.DataRecording DataRecording { get; set; }
    }
}
