using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// vevő adatok regisztrációhoz
    /// </summary>
    public class Customer
    {
        public Customer() : this("", false, "", "", "", "", "", "", "") { }

        public Customer(string mainEmail, bool newsletterToMainEmail, string country, string customerId, string customerName, string registrationNumber, string vatNumber, string euVatNumber, string signatureEntityFile)
        {
            this.MainEmail = mainEmail;

            this.NewsletterToMainEmail = newsletterToMainEmail;

            this.Country = country;

            this.CustomerId = customerId;

            this.CustomerName = customerName;

            this.RegistrationNumber = registrationNumber;

            this.VatNumber = vatNumber;

            this.EUVatNumber = euVatNumber;

            this.SignatureEntityFile = signatureEntityFile;
        }

        /// <summary>
        /// központi email cím
        /// </summary>
        public string MainEmail { get; set; }

        /// <summary>
        /// kéri a hírlevelet a központi email címre? 
        /// </summary>
        public bool NewsletterToMainEmail { get; set; }

        /// <summary>
        /// ország megnevezés
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// cégazonosító
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// cégnév
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// cégjegyzékszám
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// adószám
        /// </summary>
        public string VatNumber { get; set; }

        /// <summary>
        /// uniós adószám
        /// </summary>
        public string EUVatNumber { get; set; }

        /// <summary>
        /// aláírási címpéldány
        /// </summary>
        public string SignatureEntityFile { get; set; }
    }
}
