using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// regisztrációs adatok mint - cég, számla, (levelezési, szállítási) adatok felvitele
    /// </summary>
    public class UpdateRegistrationData
    {
        /// <summary>
        /// regisztrációs azonosító
        /// </summary>
        public string RegistrationId { get; set; }

        /// <summary>
        /// látogató azonosító
        /// </summary>
        public string VisitorId { get; set; }

        /// <summary>
        /// beállított nyelv
        /// </summary>
        public string LanguageId { get; set; }

        /// <summary>
        /// céges adatok
        /// </summary>
        public CompanyGroup.Dto.RegistrationModule.CompanyData CompanyData { get; set; }

        /// <summary>
        /// számlázási cím adatok
        /// </summary>
                        
        public CompanyGroup.Dto.RegistrationModule.InvoiceAddress InvoiceAddress { get; set; }

        /// <summary>
        /// levelezési cím adatok
        /// </summary>
        public CompanyGroup.Dto.RegistrationModule.MailAddress MailAddress { get; set; }

        /// <summary>
        /// beállította-e a nem egyezik a levelezési cím kapcsolót
        /// </summary>
        public bool ModifyMailAddress { get; set; }

        /// <summary>
        /// beállította-e a nem egyezik a szállítási cím kapcsolót
        /// </summary>
        public bool ModifyDeliveryAddress { get; set; }

        /// <summary>
        /// szállítási cím
        /// </summary>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddress DeliveryAddress { get; set; }

        /// <summary>
        /// bankszámlaszám
        /// </summary>
        public CompanyGroup.Dto.RegistrationModule.BankAccount BankAccount { get; set; }
    }
}
