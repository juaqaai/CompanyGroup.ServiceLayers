using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class UpdateDataRecording : CompanyGroup.Dto.ServiceResponse.UpdateDataRecording
    {
        public UpdateDataRecording(CompanyGroup.Dto.ServiceResponse.UpdateDataRecording updateDataRecording)
        {
            this.Message = updateDataRecording.Message;

            this.Successed = updateDataRecording.Successed;
        }
    }
}