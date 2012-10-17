using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class BankAccountToBankAccount
    {
        /// <summary>
        /// Domain regisztráció bankszámla adatok -> DTO. bankszámla adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.BankAccount MapDomainToDto(CompanyGroup.Domain.RegistrationModule.BankAccount from)
        {
            return new CompanyGroup.Dto.RegistrationModule.BankAccount() { Id = from.Id.ToString(), Part1 = from.Part1, Part2 = from.Part2, Part3 = from.Part3, RecId = from.RecId };
        }

        /// <summary>
        /// DTO. bankszámla adatok -> Domain regisztráció bankszámla adatok 
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.BankAccount MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.BankAccount from)
        {
            return new CompanyGroup.Domain.RegistrationModule.BankAccount(from.Part1, from.Part2, from.Part3, from.RecId) { Id = MongoDB.Bson.ObjectId.Parse(from.Id) };
        }
    }
}
