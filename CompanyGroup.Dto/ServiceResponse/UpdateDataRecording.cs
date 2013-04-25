using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceResponse
{
    /// <summary>
    /// adatrögzítő adatainak mentése
    /// </summary>
    public class UpdateDataRecording
    {
        public UpdateDataRecording(bool succeeded, string message, CompanyGroup.Dto.PartnerModule.Visitor visitor)
        {
            this.Succeeded = succeeded;

            this.Message = message;

            this.Visitor = visitor;
        }

        public UpdateDataRecording() : this(false, String.Empty, new CompanyGroup.Dto.PartnerModule.Visitor()) { }

        /// <summary>
        /// sikeres volt-e a művelet, vagy nem
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// a kérés nyelvétől függő hibaüzenet (ha nincs hiba, akkor üres) 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// látogató adatai
        /// </summary>
        public CompanyGroup.Dto.PartnerModule.Visitor Visitor { get; set; }
    }
}
