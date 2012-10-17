using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "UpdateDataRecording", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class UpdateDataRecording
    {

        [System.Runtime.Serialization.DataMember(Name = "RegistrationId", Order = 1)]
        public string RegistrationId { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "LanguageId", Order = 2)]
        public string LanguageId { get; set; }

        /// <summary>
        /// DataRecording
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "DataRecording", Order = 3)]
        public CompanyGroup.Dto.RegistrationModule.DataRecording DataRecording { get; set; }
    }
}
