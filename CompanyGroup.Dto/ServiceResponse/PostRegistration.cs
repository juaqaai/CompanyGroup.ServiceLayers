using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceResponse
{
    /// <summary>
    /// üres válasz objektum osztály
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "PostRegistration", Namespace = "CompanyGroup.Dto.ServiceResponse")]
    public class PostRegistration
    {
        /// <summary>
        /// sikeres volt-e a művelet, vagy nem
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Successed", Order = 1)]
        public bool Successed { get; set; }

        /// <summary>
        /// a kérés nyelvétől függő hibaüzenet (ha nincs hiba, akkor üres) 
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Message", Order = 2)]
        public string Message { get; set; }
    }
}
