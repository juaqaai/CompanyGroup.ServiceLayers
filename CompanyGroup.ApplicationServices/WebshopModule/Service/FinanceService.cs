using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CompanyGroup.ApplicationServices.WebshopModule
{
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                InstanceContextMode = InstanceContextMode.PerCall,
    //                ConcurrencyMode = ConcurrencyMode.Multiple,
    //                IncludeExceptionDetailInFaults = true),
    //                System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()]
    public class FinanceService : ServiceBase, IFinanceService
    {
        public FinanceService(CompanyGroup.Domain.WebshopModule.IFinanceRepository financeRepository,
                              CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(financeRepository, visitorRepository)
        {
        }

        private static readonly string OfferingMailSubject = Helpers.ConfigSettingsParser.GetString("OfferingMailSubject", "HRP Finance ajánlatkérés üzenet");

        private static readonly string OfferingMailHtmlTemplateFile = Helpers.ConfigSettingsParser.GetString("OfferingMailHtmlTemplateFile", "offering.html");

        private static readonly string OfferingMailTextTemplateFile = Helpers.ConfigSettingsParser.GetString("OfferingMailTextTemplateFile", "offering.txt");

        private static readonly string OfferingMailToAddress = Helpers.ConfigSettingsParser.GetString("OfferingMailToAddress", "jverebelyi@hrp.hu"); //berlet@hrpfinance.hu

        private static readonly string OfferingMailToName = Helpers.ConfigSettingsParser.GetString("OfferingMailToName", "HRP Finance");

        private static readonly string OfferingMailCcAddress = Helpers.ConfigSettingsParser.GetString("OfferingMailCcAddress", "ajuhasz@hrp.hu"); //rtokes

        private static readonly string OfferingMailCcName = Helpers.ConfigSettingsParser.GetString("OfferingMailCcName", "Tőkés Róbert");

        private static readonly string OfferingMailSmtpHost = Helpers.ConfigSettingsParser.GetString("OfferingMailSmtpHost", "195.30.7.14");

        private const string OfferItemHtml = "<tr>\n" +
                                             "<td>Termékazonosító:</td>\n" +
                                             "<td>$ProductId$</td>\n" +
                                             "</tr>\n" +
                                             "<tr>\n" +
                                             "<td>Termék neve:</td>\n" +
                                             "<td>$ProductName$</td>\n" +
                                             "</tr>\n" +
            //"<tr>\n" +
            //    "<td>Gyártó:</td>\n" +
            //    "<td>$Manufacturer$</td>\n" +
            //"</tr>\n" +
            //"<tr>\n" +
            //    "<td>Jelleg1:</td>\n" +
            //    "<td>$Category1$</td>\n" +
            //"</tr>\n" +
                                             "<tr>\n" +
                                                 "<td>Darabszám:</td>\n" +
                                                 "<td>$Quantity$</td>\n" +
                                             "</tr>\n" +
                                             "<tr>\n" +
                                                 "<td>Nettó vételára:</td>\n" +
                                                 "<td>$Price$ Ft</td>\n" +
                                             "</tr>\n" +
                                             "<tr>\n" +
                                                 "<td colspan=\"2\"><hr/></td>\n" +
                                             "</tr>\n";

        private const string OfferItemTxt = "\n" +
                                            "Termékazonosító: $ProductId$ \n" +
                                            "Termék neve: $ProductName$ \n" +
            //"Gyártó: $Manufacturer$ \n" +
            //"Jelleg1: $Category1$ \n" +
                                            "Darabszám: $Quantity$ \n" +
                                            "Nettó vételára: $Price$ Ft \n" +
                                            "________________________________________\n\n";
        /// <summary>
        /// finanszírozási ajánlat levél txt template
        /// </summary>
        private static string PlainText
        {
            get
            {
                System.IO.StreamReader sr = null;
                try
                {
                    string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + OfferingMailTextTemplateFile);

                    sr = new System.IO.StreamReader(filepath);

                    return sr.ReadToEnd();
                }
                catch
                {
                    return String.Empty;
                }
                finally
                {
                    if (sr != null) { sr.Close(); }
                }
            }
        }

        /// <summary>
        /// finanszírozási ajánlat levél html template
        /// </summary>
        private static string HtmlText
        {
            get
            {
                System.IO.StreamReader sr = null;
                try
                {
                    string filepath = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Request.PhysicalApplicationPath + OfferingMailHtmlTemplateFile);

                    sr = new System.IO.StreamReader(filepath);

                    return sr.ReadToEnd();
                }
                catch
                {
                    return String.Empty;
                }
                finally
                {
                    if (sr != null) { sr.Close(); }
                }
            }
        }

        /// <summary>
        /// finanszírozási ajánlat készítése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment CreateFinanceOffer(CompanyGroup.Dto.WebshopModule.CreateFinanceOfferRequest request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "FinanceService CreateFinanceOffer request cannot be null, or empty!");

            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "FinanceService CreateFinanceOffer VisitorId parameter cannot be null, or empty!");

            Helpers.DesignByContract.Require((request.OfferId > 0), "FinanceService CreateFinanceOffer Offer id parameter cannot be zero!");

            try
            {
                //látogató kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = visitorRepository.GetItemById(request.VisitorId);

                //kosár tartalom lekérdezése, levélküldés
                Domain.WebshopModule.FinanceOffer financeOfferToAdd = new Domain.WebshopModule.FinanceOffer()
                {
                    Address = request.Address,
                    NumOfMonth = request.NumOfMonth,
                    PersonName = request.PersonName,
                    Phone = request.Phone,
                    StatNumber = request.StatNumber
                };

                CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer = financeRepository.GetFinanceOffer(request.OfferId);

                Helpers.DesignByContract.Require((financeOffer != null), "FinanceOffer cannot be null!");

                //levélküldés
                bool sendSuccess = SendFinanceOfferMail(request, financeOffer, visitor);

                //kosár beállítása finanszírozásra elküldött státuszba, ajánlathoz szükséges adatok mentése
                financeRepository.Post(financeOffer); 

                //leasing opciók lekérdezése
                List<CompanyGroup.Domain.WebshopModule.LeasingOption> leasingOptionList = financeRepository.GetLeasingByFinancedAmount(0);

                //kalkuláció
                CompanyGroup.Domain.WebshopModule.LeasingOptions leasingOptions = new CompanyGroup.Domain.WebshopModule.LeasingOptions(this.GetMinMaxLeasingValue(), leasingOptionList);

                leasingOptions.Amount = 0;

                leasingOptions.ValidateAmount();

                CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment response = new CompanyGroup.Dto.WebshopModule.FinanceOfferFulFillment()
                {
                    LeasingOptions = new LeasingOptionsToLeasingOptions().Map(leasingOptions),
                    EmaiNotification = sendSuccess,
                    Message = ""
                };

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// finance ajánlatkérés levélküldés 
        /// </summary>
        /// <param name="financeOffer"></param>
        /// <param name="financeOffer"></param>
        /// <param name="visitor"></param>
        /// <returns></returns>
        private bool SendFinanceOfferMail(CompanyGroup.Dto.WebshopModule.CreateFinanceOfferRequest request,
                                          CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer,
                                          CompanyGroup.Domain.PartnerModule.Visitor visitor)
        {
            try
            {
                double financedAmount = 0;

                string offerItemsHtml = String.Empty;
                string offerItemsTxt = String.Empty;

                financeOffer.Items.ToList().ForEach(
                    delegate(CompanyGroup.Domain.WebshopModule.ShoppingCartItem line)
                    {
                        financedAmount += (line.Quantity * line.CustomerPrice);

                        offerItemsHtml += FinanceService.OfferItemHtml.Replace("$ProductId$", line.ProductId)
                            //.Replace("$Manufacturer$", line.Structure.Manufacturer.ManufacturerName)
                            //.Replace("$Category1$", line.Structure.Category1.CategoryName)
                                                                    .Replace("$ProductName$", line.ProductName)
                                                                    .Replace("$Quantity$", Helpers.ConvertData.ConvertIntToString(line.Quantity))
                                                                    .Replace("$Price$", Convert.ToString(line.CustomerPrice));

                        offerItemsTxt += FinanceService.OfferItemTxt.Replace("$ProductId$", line.ProductId)
                            //.Replace("$Manufacturer$", line.Structure.Manufacturer.ManufacturerName)
                            //.Replace("$Category1$", line.Structure.Category1.CategoryName)
                                                                    .Replace("$ProductName$", line.ProductName)
                                                                    .Replace("$Quantity$", Helpers.ConvertData.ConvertIntToString(line.Quantity))
                                                                    .Replace("$Price$", Convert.ToString(line.CustomerPrice));
                    });

                string tmpHtml = FinanceService.HtmlText;
                string html = tmpHtml.Replace("$PersonName$", financeOffer.PersonName)
                                        .Replace("$Address$", financeOffer.Address)
                                        .Replace("$Phone$", financeOffer.Phone)
                                        .Replace("$StatNumber$", financeOffer.StatNumber)
                                        .Replace("$NumOfMonth$", Helpers.ConvertData.ConvertIntToString(financeOffer.NumOfMonth))
                                        .Replace("$FinancedAmount$", Convert.ToString(financedAmount))
                                        .Replace("$SentDate$", DateTime.Now.ToShortDateString())
                                        .Replace("$OfferItems$", offerItemsHtml)
                                        .Replace("$CustName$", visitor.CustomerName)
                                        .Replace("$CustPersonName$", visitor.PersonName)
                                        .Replace("$CustPhone$", String.IsNullOrEmpty(visitor.PersonName) ? "" : "")
                                        .Replace("$CustEmail$", String.IsNullOrEmpty(visitor.PersonName) ? "" : visitor.PersonName);

                string tmpPlain = FinanceService.PlainText;
                string plain = tmpPlain.Replace("$PersonName$", financeOffer.PersonName)
                                        .Replace("$Address$", financeOffer.Address)
                                        .Replace("$Phone$", financeOffer.Phone)
                                        .Replace("$StatNumber$", financeOffer.StatNumber)
                                        .Replace("$NumOfMonth$", Helpers.ConvertData.ConvertIntToString(financeOffer.NumOfMonth))
                                        .Replace("$FinancedAmount$", Convert.ToString(financedAmount))
                                        .Replace("$SentDate$", DateTime.Now.ToShortDateString())
                                        .Replace("$OfferItems$", offerItemsHtml)
                                        .Replace("$CustName$", visitor.CustomerName)
                                        .Replace("$CustPersonName$", visitor.PersonName)
                                        .Replace("$CustPhone$", String.IsNullOrEmpty(visitor.PersonName) ? "" : "")
                                        .Replace("$CustEmail$", String.IsNullOrEmpty(visitor.PersonName) ? "" : "");

                MailMergeLib.MailMergeMessage mmm = new MailMergeLib.MailMergeMessage(OfferingMailSubject, plain, html);

                mmm.BinaryTransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                mmm.CharacterEncoding = System.Text.Encoding.GetEncoding("iso-8859-2");
                mmm.CultureInfo = new System.Globalization.CultureInfo("hu-HU");
                mmm.FileBaseDir = System.IO.Path.GetFullPath(System.Web.HttpContext.Current.Server.MapPath("../"));
                mmm.Priority = System.Net.Mail.MailPriority.Normal;
                mmm.TextTransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.To, "<" + OfferingMailToAddress + ">", OfferingMailToName, System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.From, "<webadmin@hrp.hu>", "web adminisztrátor csoport", System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.CC, "<" + OfferingMailCcAddress + ">", OfferingMailCcName, System.Text.Encoding.Default));
                mmm.MailMergeAddresses.Add(new MailMergeLib.MailMergeAddress(MailMergeLib.MailAddressType.Bcc, "<ajuhasz@hrp.hu>", "Juhász Attila", System.Text.Encoding.Default));

                //mail sender
                MailMergeLib.MailMergeSender mailSender = new MailMergeLib.MailMergeSender();

                //esemenykezelok beallitasa, ha van
                mailSender.OnSendFailure += new EventHandler<MailMergeLib.MailSenderSendFailureEventArgs>(delegate(object obj, MailMergeLib.MailSenderSendFailureEventArgs args)
                //( ( obj, args ) =>
                {
                    string errorMsg = args.Error.Message;
                    MailMergeLib.MailMergeMessage.MailMergeMessageException ex = args.Error as MailMergeLib.MailMergeMessage.MailMergeMessageException;
                    if (ex != null && ex.Exceptions.Count > 0)
                    {
                        errorMsg = string.Format("{0}", ex.Exceptions[0].Message);
                    }
                    string text = string.Format("Error: {0}", errorMsg);

                });

                mailSender.LocalHostName = Environment.MachineName; //"mail." + 
                mailSender.MaxFailures = 1;
                mailSender.DelayBetweenMessages = 1000;
                string messageOutputDir = System.IO.Path.GetTempPath() + @"\mail";
                if (!System.IO.Directory.Exists(messageOutputDir))
                {
                    System.IO.Directory.CreateDirectory(messageOutputDir);
                }
                mailSender.MailOutputDirectory = messageOutputDir;
                mailSender.MessageOutput = MailMergeLib.MessageOutput.SmtpServer;  // change to MessageOutput.Directory if you like

                // smtp details
                mailSender.SmtpHost = OfferingMailSmtpHost;
                mailSender.SmtpPort = 25;
                //mailSender.SetSmtpAuthentification( "username", "password" );

                mailSender.Send(mmm);

                return true;
            }
            catch (Exception ex)
            {
                //throw new ApplicationException("A levél elküldése nem sikerült", ex);
                return false;
            }

        }

    }
}
