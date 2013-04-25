using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// adatmódosítás view model
    /// </summary>
    public class UpdateDataRecording : CompanyGroup.Dto.ServiceResponse.UpdateDataRecording
    {
        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="updateDataRecording"></param>
        public UpdateDataRecording(CompanyGroup.Dto.ServiceResponse.UpdateDataRecording updateDataRecording) : base(updateDataRecording.Succeeded, updateDataRecording.Message, updateDataRecording.Visitor)
        {

        }
    }
}