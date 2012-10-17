using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// vevő entitás
    /// CustomerId	CompanyRegisterNumber	Newsletter	SignatureEntityFile	CustomerName	VatNumber	EuVatNumber	InvoiceCountry	InvoiceCity	InvoiceStreet	InvoiceZipCode	InvoicePhone	InvoiceCellularPhone	InvoiceEmail	MailCountry	MailCity	MailStreet	MailZipCode	dtDateTime
    /// </summary>
    public class Customer : CompanyGroup.Domain.Core.Entity, IValidatableObject
    {
        public Customer(string customerId, string companyRegisterNumber, bool newsletter, string signatureEntityFile, string customerName, string vatNumber, string	euVatNumber,
            string invoiceCountry, string invoiceCity, string invoiceStreet, string invoiceZipCode, string invoicePhone, string invoiceCellularPhone, 
            string invoiceEmail, string mailCountry, string mailCity, string mailStreet, string	mailZipCode, DateTime createdDate)
        {
            this.CustomerId = customerId;

            this.CompanyRegisterNumber = companyRegisterNumber;

            this.Newsletter = newsletter;

            this.SignatureEntityFile = signatureEntityFile;

            this.CustomerName = customerName;

            this.VatNumber = vatNumber;

            this.SplitVatNumber();

            this.EUVatNumber = euVatNumber;

            this.InvoiceCountry = invoiceCountry;

            this.InvoiceCity = invoiceCity;

            this.InvoiceStreet = invoiceStreet;

            this.InvoiceZipCode = invoiceZipCode;

            this.InvoicePhone = invoicePhone;

            this.Email = invoiceEmail;

            this.MailCity = mailCity;

            this.MailCountry = mailCountry;

            this.MailZipCode = mailZipCode;

            this.MailStreet = mailStreet;
        }

        public Customer()
            : this("", "", false, "", "", "", "",
                "", "", "", "", "", "",
                "", "", "", "", "", DateTime.MinValue) { }

        /// <summary>
        /// vevőkód
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// regisztrációs szám
        /// </summary>
        public string CompanyRegisterNumber { get; set; }

        /// <summary>
        /// hírlevél feliratkozás
        /// </summary>
        public bool Newsletter { get; set; }

        /// <summary>
        /// aláírási címpéldány
        /// </summary>
        public string SignatureEntityFile { get; set; }

        /// <summary>
        /// vevő neve
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// adószám
        /// </summary>
        public string VatNumber { get; set; }

        public string VatNumberPart1 { set; get; }

        public string VatNumberPart2 { set; get; }

        public string VatNumberPart3 { set; get; }


        /// <summary>
        /// részlánc összefűzés
        /// <returns></returns>
        public string ConcatVatNumberParts()
        {
            return String.Format("{0}-{1}-{2}", this.VatNumberPart1, this.VatNumberPart2, this.VatNumberPart3);
        }

        /// <summary> 
        /// karakterlánc obj.-ba konvertálása
        /// </summary>
        /// <param name="sItem"></param>
        /// <returns></returns>
        public void SplitVatNumber()
        {
            string[] arr = this.VatNumber.Split('-');

            if (arr.Length.Equals(3))
            {
                this.VatNumberPart1 = arr[0];
                this.VatNumberPart2 = arr[1];
                this.VatNumberPart3 = arr[2];
            }
        }

        /// <summary>
        /// EU adószám
        /// </summary>
        public string EUVatNumber { get; set; }

        /// <summary>
        /// számlázási ország
        /// </summary>
        public string InvoiceCountry { get; set; }

        /// <summary>
        /// számlázási város
        /// </summary>
        public string InvoiceCity { get; set; }

        /// <summary>
        /// számlázási utca
        /// </summary>
        public string InvoiceStreet { get; set; }

        /// <summary>
        /// számlázási irányítószám 
        /// </summary>
        public string InvoiceZipCode { get; set; }

        /// <summary>
        /// számlázási telefonszám
        /// </summary>
        public string InvoicePhone { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// város
        /// </summary>
        public string MailCity { get; set; }

        /// <summary>
        /// ország
        /// </summary>
        public string MailCountry { get; set; }

        /// <summary>
        /// irányítószám
        /// </summary>
        public string MailZipCode { get; set; }

        /// <summary>
        /// utca, házszám
        /// </summary>
        public string MailStreet { get; set; }

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();


            if (String.IsNullOrEmpty(CustomerId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "CustomerId" }));
            }

            return validationResults;
        }
    }
}
