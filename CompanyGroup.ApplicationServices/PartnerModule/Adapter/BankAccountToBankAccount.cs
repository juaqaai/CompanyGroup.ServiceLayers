using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class BankAccountToBankAccount
    {
        /// <summary>
        /// Domain vevő bankszámla adatok -> DTO. bankszámla adatok   
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.BankAccount Map(CompanyGroup.Domain.PartnerModule.BankAccount from)
        {
            return new CompanyGroup.Dto.RegistrationModule.BankAccount() { Id = String.Empty, Part1 = from.Part1, Part2 = from.Part2, Part3 = from.Part3, RecId = from.RecId };
        }
    }
}
