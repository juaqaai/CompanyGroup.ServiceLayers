using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class DataRecording : CompanyGroup.Dto.RegistrationModule.DataRecording
    {
        public DataRecording(CompanyGroup.Dto.RegistrationModule.DataRecording dataRecording)
        {
            this.Email = dataRecording.Email;
            this.Name = dataRecording.Name;
            this.Phone = dataRecording.Phone;
        }

        public DataRecording() { }
    }
}
