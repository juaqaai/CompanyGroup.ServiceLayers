using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class DataRecordingToDataRecording
    {
        /// <summary>
        /// Domain regisztráló adatok -> DTO. regisztráló adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.DataRecording MapDomainToDto(CompanyGroup.Domain.RegistrationModule.DataRecording from)
        {
            return new CompanyGroup.Dto.RegistrationModule.DataRecording() { Email = from.Email, Name = from.Name, Phone = from.Phone };
        }

        /// <summary>
        /// Dto. regisztráló adatok -> Domain regisztráló adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.DataRecording MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.DataRecording from)
        {
            return new CompanyGroup.Domain.RegistrationModule.DataRecording() { Email = from.Email, Name = from.Name, Phone = from.Phone };
        }
    }
}
