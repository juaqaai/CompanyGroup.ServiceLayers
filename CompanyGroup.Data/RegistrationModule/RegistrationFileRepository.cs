using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.RegistrationModule
{

    /// <summary>
    /// regisztrációs filet kezelő repository   
    /// </summary>
    public class RegistrationFileRepository : CompanyGroup.Domain.RegistrationModule.IRegistrationFileRepository
    {

        /// <summary>
        /// regisztrációs adatok
        /// </summary>
        private CompanyGroup.Domain.RegistrationModule.Registration registration;

        /// <summary>
        /// konstruktor regisztrációs adatokkal  
        /// </summary>
        /// <param name="registration"></param>
        public RegistrationFileRepository(CompanyGroup.Domain.RegistrationModule.Registration registration)
        {
            Helpers.DesignByContract.Require((registration != null), "Registration data cannot be null or empty!");

            this.registration = registration;
        }

        /// <summary>
        /// regisztrációs file elkészítése  
        /// </summary>
        /// <param name="registrationFileNameWithPath"></param>
        /// <param name="htmlContent"></param>
        public void CreateRegistrationFile(string registrationFileNameWithPath, string htmlContent)
        {

//using (FileStream fs = new FileStream("test.htm", FileMode.Create)) 
//{ 
//    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8)) 
//    { 
//        w.WriteLine("<H1>Hello</H1>"); 
//    } 
//} 

            try
            {
                Helpers.DesignByContract.Require(!String.IsNullOrEmpty(registrationFileNameWithPath), "Registration file name with path cannot be null or empty!");

                System.IO.FileStream fs = new System.IO.FileStream(registrationFileNameWithPath, System.IO.FileMode.Create, System.IO.FileAccess.Write); //System.IO.FileShare.Write,

                System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fs, System.Text.Encoding.GetEncoding(28592));

                System.IO.StringWriter stringWriter = new System.IO.StringWriter();

                stringWriter.Write(htmlContent);

                System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(stringWriter);

                streamWriter.WriteLine(stringWriter.ToString());

                streamWriter.Close();

            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// template-ből generált regisztrációs adatok html string-ben
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <param name="registrationHtml"></param>
        /// <returns></returns>
        public string RenderRegistrationDataToHtml(string contractNumber, string registrationHtml)
        {
            string bankAccountHtml = String.Empty;

            registration.BankAccountList.ForEach(x =>
            {
                bankAccountHtml += (!String.IsNullOrEmpty(x.Number)) ? String.Format("{0} <br/>", x.Number) : String.Format("{0} - {1} - {2} <br/>", x.Part1, x.Part2, x.Part3);
            });

            string deliveryAddressHtml = String.Empty;

            registration.DeliveryAddressList.ForEach(x =>
            {
                string template = DeliveryAddressTemplate();

                deliveryAddressHtml += template.Replace("$DeliveryAddressZipCode$", x.ZipCode)
                                               .Replace("$DeliveryAddressCountryRegionId$", x.CountryRegionId)
                                               .Replace("$DeliveryAddressCity$", x.City)
                                               .Replace("$DeliveryAddressStreet$", x.Street);
            });

            string contactPersonHtml = String.Empty;

            registration.ContactPersonList.ForEach(x =>
            {
                string template = ContactPersonTemplate();
                contactPersonHtml += template.Replace("$ContactPersonId$", x.ContactPersonId)
                                                .Replace("$ContactPersonFirstName$", x.FirstName)
                                                .Replace("$ContactPersonLastName$", x.LastName)
                                                .Replace("$ContactPersonTelephone$", x.Telephone)
                                                .Replace("$ContactPersonEmail$", x.Email)
                                                .Replace("$ContactPersonPassword$", x.Password)
                                                .Replace("$ContactPersonInvoiceInfo$", Helpers.ConvertData.ConvertBoolToString(x.InvoiceInfo, "igen", "nem"))
                                                .Replace("$ContactPersonPriceListDownload$", Helpers.ConvertData.ConvertBoolToString(x.PriceListDownload, "igen", "nem"))
                                                .Replace("$ContactPersonAllowOrder$", Helpers.ConvertData.ConvertBoolToString(x.AllowOrder, "igen", "nem"))
                                                .Replace("$ContactPersonAllowReceiptOfGoods$", Helpers.ConvertData.ConvertBoolToString(x.AllowReceiptOfGoods, "igen", "nem"))
                                                .Replace("$ContactPersonNewsletter$", Helpers.ConvertData.ConvertBoolToString(x.Newsletter, "igen", "nem"))
                                                .Replace("$ContactPersonEmailArriveOfGoods$", Helpers.ConvertData.ConvertBoolToString(x.EmailArriveOfGoods, "igen", "nem"))
                                                .Replace("$ContactPersonEmailOfDelivery$", Helpers.ConvertData.ConvertBoolToString(x.EmailOfDelivery, "igen", "nem"))
                                                .Replace("$ContactPersonEmailOfOrderConfirm$", Helpers.ConvertData.ConvertBoolToString(x.EmailOfOrderConfirm, "igen", "nem"));
            });

            string webAdminPositionsHtml = String.Empty;

            registration.WebAdministrator.Positions.ForEach(x =>
            {
                webAdminPositionsHtml += String.Format("{0} <br/>", x);
            });

            string html = registrationHtml.Replace("$ContractNumber$", contractNumber)
                                          .Replace("$CompanyName$", registration.CompanyData.CustomerName)
                                          .Replace("$InvoiceAddressZipCode$", registration.InvoiceAddress.ZipCode)
                                          .Replace("$InvoiceAddressCity$", registration.InvoiceAddress.City)
                                          .Replace("$InvoiceAddressStreet$", registration.InvoiceAddress.Street)
                                          .Replace("$VatNumber$", registration.CompanyData.VatNumber)
                                          .Replace("$CompanyRegisterNumber$", registration.CompanyData.RegistrationNumber)
                                          .Replace("$DataRecordingName$", registration.DataRecording.Name)
                                          .Replace("$DataRecordingEmail$", registration.DataRecording.Email)
                                          .Replace("$DataRecordingPhone$", registration.DataRecording.Phone)
                                          .Replace("$CompanyDataCustomerName$", registration.CompanyData.CustomerName)
                                          .Replace("$CompanyDataRegistrationNumber$", registration.CompanyData.RegistrationNumber)
                                          .Replace("$CompanyDataVatNumber$", registration.CompanyData.VatNumber)
                                          .Replace("$CompanyDataEUVatNumber$", registration.CompanyData.EUVatNumber)
                                          .Replace("$BankAccounts$", bankAccountHtml)
                                          .Replace("$CompanyDataSignatureEntityFile$", registration.CompanyData.SignatureEntityFile)
                                          .Replace("$CompanyDataNewsletterToMainEmail$", Helpers.ConvertData.ConvertBoolToString(registration.CompanyData.NewsletterToMainEmail, "igen", "nem"))
                                          .Replace("$WebAdministratorFirstName$", registration.WebAdministrator.FirstName)
                                          .Replace("$WebAdministratorLastName$", registration.WebAdministrator.LastName)
                                          .Replace("$WebAdministratorTelephone$", registration.WebAdministrator.Telephone)
                                          .Replace("$WebAdministratorEmail$", registration.WebAdministrator.Email)
                                          .Replace("$WebAdministratorInvoiceInfo$", Helpers.ConvertData.ConvertBoolToString(registration.WebAdministrator.InvoiceInfo, "igen", "nem"))
                                          .Replace("$WebAdministratorPriceListDownload$", Helpers.ConvertData.ConvertBoolToString(registration.WebAdministrator.PriceListDownload, "igen", "nem"))
                                          .Replace("$WebAdministratorAllowOrder$", Helpers.ConvertData.ConvertBoolToString(registration.WebAdministrator.AllowOrder, "igen", "nem"))
                                          .Replace("$WebAdministratorAllowReceiptOfGoods$", Helpers.ConvertData.ConvertBoolToString(registration.WebAdministrator.AllowReceiptOfGoods, "igen", "nem"))
                                          .Replace("$WebAdministratorPositions$", webAdminPositionsHtml)
                                          .Replace("$WebAdministratorNewsletter$", Helpers.ConvertData.ConvertBoolToString(registration.WebAdministrator.Newsletter, "igen", "nem"))
                                          .Replace("$WebAdministratorEmailArriveOfGoods$", Helpers.ConvertData.ConvertBoolToString(registration.WebAdministrator.EmailArriveOfGoods, "igen", "nem"))
                                          .Replace("$WebAdministratorEmailOfDelivery$", Helpers.ConvertData.ConvertBoolToString(registration.WebAdministrator.EmailOfDelivery, "igen", "nem"))
                                          .Replace("$WebAdministratorEmailOfOrderConfirm$", Helpers.ConvertData.ConvertBoolToString(registration.WebAdministrator.EmailOfOrderConfirm, "igen", "nem"))
                                          .Replace("$InvoiceAddressCountryRegionId$", registration.InvoiceAddress.Country)
                                          .Replace("$InvoiceAddressCity$", registration.InvoiceAddress.City)
                                          .Replace("$InvoiceAddressStreet$", registration.InvoiceAddress.Street)
                                          .Replace("$InvoiceAddressPhone$", registration.InvoiceAddress.Phone)
                                          .Replace("$MailAddressZipCode$", registration.MailAddress.ZipCode)
                                          .Replace("$MailAddressCountryRegionId$", registration.MailAddress.Country)
                                          .Replace("$MailAddressCity$", registration.MailAddress.City)
                                          .Replace("$MailAddressStreet$", registration.MailAddress.Street)
                                          .Replace("$DeliveryAddresses$", deliveryAddressHtml)
                                          .Replace("$ContactPersons$", contactPersonHtml)
                                          .Replace("$DateTimeNow$", DateTime.Now.ToShortDateString());

            return html;
        }

        /// <summary>
        /// beolvassa a fizikai elérési útról a regisztrációs html template file-t
        /// </summary>
        /// <param name="registrationTemplateFileNameWithPath">regisztrációs template file elérési úttal</param>
        public string ReadRegistrationHtmlTemplate(string registrationTemplateFileNameWithPath)
        {
            CompanyGroup.Helpers.FileReader fileReader = new CompanyGroup.Helpers.FileReader(registrationTemplateFileNameWithPath, 28592);

            return fileReader.ReadToEnd();

            //System.IO.StreamReader sr = null;
            //try
            //{
            //    sr = new System.IO.StreamReader(registrationTemplateFileNameWithPath);

            //    return sr.ReadToEnd();
            //}
            //catch
            //{
            //    return String.Empty;
            //}
            //finally
            //{
            //    if (sr != null) { sr.Close(); }
            //}
        }

        private static string DeliveryAddressTemplate()
        {
            return "<br/><span>Irányítószám: </span>$DeliveryAddressZipCode$ <br/>" +
                   "<span>Ország: </span>$DeliveryAddressCountryRegionId$ <br/>" +
                   "<span>Város: </span>$DeliveryAddressCity$ <br/>" +
                   "<span>Utca, házszám:</span>$DeliveryAddressStreet$ <br/><br/>";
        }

        private static string ContactPersonTemplate()
        {
            return "<span>Azonosító: </span>$ContactPersonId$<br/>" +
                   "<span>Vezetéknév: </span>$ContactPersonFirstName$<br/>" +
                   "<span>Keresztnév: </span>$ContactPersonLastName$<br/>" +
                   "<span>Telefon: </span>$ContactPersonTelephone$<br/>" +
                   "<span>Email: </span>$ContactPersonEmail$<br/>" +
                   "<span>Jelszó: </span>$ContactPersonPassword$<br/>" +
                   "<br/>" +
                   "<span><strong>Jogosultságok: </strong></span><br/>" +
                   "<span>Számlainformáció: </span>$ContactPersonInvoiceInfo$<br/>" +
                   "<span>Árlistát letölthet: </span>$ContactPersonPriceListDownload$<br/>" +
                   "<span>Árut Rendelhet: </span>$ContactPersonAllowOrder$ <br/>" +
                   "<span>Árut átvehet: </span>$ContactPersonAllowReceiptOfGoods$ <br/>" +
                   "<br/>" +
                   "<span><strong>Pozíció: </strong></span><br/> " +
                   "<br/>" +
                   "<span><strong>Hírlevél: </strong></span>$ContactPersonNewsletter$<br/>" +
                   "<br/>   " +
                   "<span><strong>Automata e-mail értesítések:</strong></span><br/>   " +
                   "<span>Email értesítés árukiszállításról: </span>$ContactPersonEmailArriveOfGoods$<br/>" +
                   "<span>Email értesítés árubeérkezésről: </span>$ContactPersonEmailOfDelivery$<br/>" +
                   "<span>Email értesítés rendelésről: </span>$ContactPersonEmailOfOrderConfirm$<br/><br/>";
        }
    }
}
