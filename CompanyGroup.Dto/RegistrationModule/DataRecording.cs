using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.RegistrationModule
{
    [System.Runtime.Serialization.DataContract(Name = "DataRecording", Namespace = "CompanyGroup.Dto.RegistrationModule")]
    public class DataRecording
    {
        /// <summary>
        /// email
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Email", Order = 1)]
        public string Email { get; set; }

        /// <summary>
        /// név
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Name", Order = 2)]
        public string Name { get; set; }

        /// <summary>
        /// telefon
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Phone", Order = 3)]
        public string Phone { get; set; }
    }
}
