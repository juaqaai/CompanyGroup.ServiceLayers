using System;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// regisztrációs adatok
    /// CompanyRegisterNumber	CustomerName	    VatNumber	    EuVatNumber	InvoiceCountry	InvoiceCity	    InvoiceStreet	            InvoiceZipCode	DataAreaId
	///                         iPon Computer Kft.	13392327-2-41	HU13392327	HU	            Budapest        II.ker	Búzavirág utca 9.	1025	        hrp
    /// </summary>
    public class CustomerContractData
    {
        public CustomerContractData(string companyRegisterNumber, string customerName, string vatNumber, string	euVatNumber,
                                    string invoiceCountry, string invoiceCity, string invoiceStreet, string invoiceZipCode)
        {
            this.CompanyRegisterNumber = companyRegisterNumber;

            this.CustomerName = customerName;

            this.VatNumber = vatNumber;

            this.EUVatNumber = euVatNumber;

            this.InvoiceCountry = invoiceCountry;

            this.InvoiceCity = invoiceCity;

            this.InvoiceStreet = invoiceStreet;

            this.InvoiceZipCode = invoiceZipCode;
        }

        public CustomerContractData() : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,  String.Empty) { }

        /// <summary>
        /// regisztrációs szám
        /// </summary>
        public string CompanyRegisterNumber { get; set; }

        /// <summary>
        /// vevő neve
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// adószám
        /// </summary>
        public string VatNumber { get; set; }

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
        /// kis / vagy módosításról van-e szó? 1: új, 2: módosít, 3: töröl, 4: kis módosítás
        /// csak bejelentkezett státusszal hívható 
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public int CalculateRegistrationMethod(CompanyGroup.Domain.RegistrationModule.Registration registration)
        {
            bool isEqual = (//this.CompanyRegisterNumber.Equals(registration.CompanyData.RegistrationNumber, StringComparison.OrdinalIgnoreCase)
                        this.CustomerName.Equals(registration.CompanyData.CustomerName, StringComparison.OrdinalIgnoreCase)
                        //&& this.EUVatNumber.Equals(registration.CompanyData.EUVatNumber, StringComparison.OrdinalIgnoreCase)
                        && this.InvoiceCity.Equals(registration.InvoiceAddress.City, StringComparison.OrdinalIgnoreCase)
                        && this.InvoiceCountry.Equals(registration.InvoiceAddress.Country, StringComparison.OrdinalIgnoreCase)
                        && this.InvoiceStreet.Equals(registration.InvoiceAddress.Street, StringComparison.OrdinalIgnoreCase)
                        && this.InvoiceZipCode.Equals(registration.InvoiceAddress.ZipCode, StringComparison.OrdinalIgnoreCase)
                        && this.VatNumber.Equals(registration.CompanyData.VatNumber, StringComparison.OrdinalIgnoreCase));

            return isEqual ? 4 : 2;
        }
    }
}
