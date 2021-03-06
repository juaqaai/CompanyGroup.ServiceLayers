﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;


namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    //[ServiceBehavior(UseSynchronizationContext = false,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 IncludeExceptionDetailInFaults = true),
    //                 System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] //create instance and inject dependencies using unity container
    public class RegistrationService : ServiceCoreBase, IRegistrationService 
    {
        private static readonly string RegistrationFilePath = Helpers.ConfigSettingsParser.GetString("RegistrationFilePath", @"c:\projects\2012\CompanyGroup.WebApi\App_Data\");

        private static readonly string RegistrationTemplateFileName = Helpers.ConfigSettingsParser.GetString("RegistrationTemplateFileName", "registrationcontract.html");

        private static readonly string RegistrationTemplateFilePath = Helpers.ConfigSettingsParser.GetString("RegistrationTemplateFilePath", @"c:\projects\2012\CompanyGroup.WebApi\App_Data\Templates\");

        private CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository;

        private CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository;

        public RegistrationService(CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository, 
                                   CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository, 
                                   CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (registrationRepository == null)
            {
                throw new ArgumentNullException("RegistrationRepository");
            }

            this.registrationRepository = registrationRepository;

            this.customerRepository = customerRepository;
        }

        /// <summary>
        /// kulcs alapján a megkezdett regisztrációs adatok kiolvasása a cacheDb-ből
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.Registration GetByKey(CompanyGroup.Dto.ServiceRequest.GetRegistrationByKey request)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(request.Id))
                {
                    return new CompanyGroup.Dto.RegistrationModule.Registration();
                }

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.Id);

                //ha nincs az azonosítóval rendelkező regisztráció, akkor új regisztráció létrehozása szükséges
                if (registration == null)
                {
                    registration = CompanyGroup.Domain.RegistrationModule.Factory.CreateRegistration();
                }

                registration.BankAccountList.ForEach(x => x.SplitBankAccount());

                CompanyGroup.Dto.RegistrationModule.Registration response = new RegistrationToRegistration().MapDomainToDto(registration);

                //látogató adatok lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                response.Visitor = new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// új regisztráció hozzáadása 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.Registration AddNew(CompanyGroup.Dto.ServiceRequest.AddNewRegistration request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration datarecording cannot be null!");

                CompanyGroup.Domain.RegistrationModule.Registration registration = null;

                //be kell-e tölteni a UI-on az adatokat?
                bool loadData = false;

                //bejelentkezett felhasználó lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = base.GetVisitor(request.VisitorId);

                //ha be van jelentkezve a felhasználó, akkor le kell kérdezni a korábbi regisztrációs adatokat
                if (visitor.IsValidLogin && (String.IsNullOrEmpty(request.RegistrationId) || MongoDB.Bson.ObjectId.Empty.ToString().Equals(request.RegistrationId)))
                {
                    registration = new CompanyGroup.Domain.RegistrationModule.Registration();

                    registration.DataRecording = new Domain.RegistrationModule.DataRecording();

                    List<CompanyGroup.Domain.PartnerModule.BankAccount> bankAccounts = customerRepository.GetBankAccounts(visitor.CustomerId, CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                    List<CompanyGroup.Domain.PartnerModule.ContactPerson> contactPersons = customerRepository.GetContactPersons(visitor.CustomerId, CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                    CompanyGroup.Domain.PartnerModule.Customer customer = customerRepository.GetCustomer(visitor.CustomerId, CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                    List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddresses = customerRepository.GetDeliveryAddress(visitor.CustomerId, CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                    CompanyGroup.Domain.PartnerModule.MailAddress mailAddress = customerRepository.GetMailAddress(visitor.CustomerId, CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                    //registration.BankAccountList = new List<Domain.RegistrationModule.BankAccount>();

                    registration.BankAccountList = bankAccounts.ConvertAll<CompanyGroup.Domain.RegistrationModule.BankAccount>(x => { return new CompanyGroup.Domain.RegistrationModule.BankAccount(x.Number, x.RecId); });

                    registration.ContactPersonList = contactPersons.FindAll(x => x.WebAdmin == false).ConvertAll<CompanyGroup.Domain.RegistrationModule.ContactPerson>(x => { 
                        
                        return new CompanyGroup.Domain.RegistrationModule.ContactPerson(x.ContactPersonId, 
                                                                                        x.LastName, 
                                                                                        x.FirstName, 
                                                                                        x.Email, 
                                                                                        x.Telephone, 
                                                                                        x.Telephone, 
                                                                                        x.AllowOrder, 
                                                                                        x.AllowReceiptOfGoods, 
                                                                                        x.SmsArriveOfGoods, 
                                                                                        x.SmsOrderConfirm, 
                                                                                        x.SmsOfDelivery, 
                                                                                        x.EmailArriveOfGoods, 
                                                                                        x.EmailOfOrderConfirm, 
                                                                                        x.EmailOfDelivery, 
                                                                                        x.WebAdmin, 
                                                                                        x.PriceListDownload, 
                                                                                        x.InvoiceInfo, 
                                                                                        x.UserName, 
                                                                                        x.Password, 
                                                                                        x.LeftCompany,
                                                                                        x.Newsletter,
                                                                                        x.RecId, 
                                                                                        x.RefRecId, 
                                                                                        x.Positions); 
                    });

                    registration.CompanyData = new Domain.RegistrationModule.CompanyData(customer.InvoiceCountry, customer.CustomerId, customer.CustomerName, customer.EUVatNumber, customer.Email, 
                                                                                         customer.Newsletter, customer.CompanyRegisterNumber, customer.SignatureEntityFile, customer.VatNumber);

                    registration.CompanyId = customer.CustomerId;

                    registration.DeliveryAddressList = deliveryAddresses.ConvertAll<CompanyGroup.Domain.RegistrationModule.DeliveryAddress>(x => { 
                        
                        return new CompanyGroup.Domain.RegistrationModule.DeliveryAddress(x.RecId, x.City, x.ZipCode, x.Street, x.CountryRegionId); 
                    
                    });

                    registration.MailAddress = new Domain.RegistrationModule.MailAddress(customer.MailCity, customer.MailCountry, customer.MailZipCode, customer.MailStreet);

                    registration.InvoiceAddress = new Domain.RegistrationModule.InvoiceAddress(customer.InvoiceCity, customer.InvoiceCountry, customer.InvoiceZipCode, customer.InvoiceStreet, customer.InvoicePhone);


                    CompanyGroup.Domain.PartnerModule.ContactPerson webAdmin = contactPersons.FirstOrDefault(x => x.WebAdmin == true);

                    

                    registration.WebAdministrator = (webAdmin == null) ? new CompanyGroup.Domain.RegistrationModule.WebAdministrator() :
                        
                                                                         new CompanyGroup.Domain.RegistrationModule.WebAdministrator(webAdmin.ContactPersonId,
                                                                                                                webAdmin.LastName,
                                                                                                                webAdmin.FirstName,
                                                                                                                webAdmin.Email,
                                                                                                                webAdmin.Telephone,
                                                                                                                webAdmin.Telephone,
                                                                                                                webAdmin.AllowOrder,
                                                                                                                webAdmin.AllowReceiptOfGoods,
                                                                                                                webAdmin.SmsArriveOfGoods,
                                                                                                                webAdmin.SmsOrderConfirm,
                                                                                                                webAdmin.SmsOfDelivery,
                                                                                                                webAdmin.EmailArriveOfGoods,
                                                                                                                webAdmin.EmailOfOrderConfirm,
                                                                                                                webAdmin.EmailOfDelivery, 
                                                                                                                webAdmin.PriceListDownload,
                                                                                                                webAdmin.InvoiceInfo,
                                                                                                                webAdmin.UserName,
                                                                                                                webAdmin.Password,
                                                                                                                webAdmin.LeftCompany,
                                                                                                                webAdmin.Newsletter,
                                                                                                                webAdmin.RecId,
                                                                                                                webAdmin.RefRecId, 
                                                                                                                webAdmin.Positions);
                    
                    registration.VisitorId = visitor.VisitorId;

                    registration.Status = Domain.RegistrationModule.RegistrationStatus.Created;

                    loadData = true;
                }

                //ha nem üres a kérésben szereplő regisztrációs azonosító, akkor annak lekérdezése történik a cacheDb-ből
                if (!String.IsNullOrEmpty(request.RegistrationId) && !MongoDB.Bson.ObjectId.Empty.ToString().Equals(request.RegistrationId))
                {
                    registration = registrationRepository.GetByKey(request.RegistrationId);
                }

                //ha nincs megkezdett regisztráció 
                if (registration == null || MongoDB.Bson.ObjectId.Empty.Equals(registration.Id))
                {
                    //üres regisztráció létrehozása
                    CompanyGroup.Domain.RegistrationModule.Registration newRegistration = (registration == null) ? CompanyGroup.Domain.RegistrationModule.Factory.CreateRegistration() : registration;

                    //új regisztrációs azonosító létrehozása
                    newRegistration.Id = MongoDB.Bson.ObjectId.GenerateNewId();

                    newRegistration.CompanyId = visitor.IsValidLogin ? visitor.CustomerId : String.Empty;

                    newRegistration.PersonId = visitor.IsValidLogin ? visitor.PersonId : String.Empty;

                    newRegistration.VisitorId = visitor.IsValidLogin ? request.VisitorId : String.Empty;

                    //új regisztráció mentés 
                    registrationRepository.Add(newRegistration);

                    //új regisztráció visszaolvasás
                    registration = registrationRepository.GetByKey(newRegistration.Id.ToString());

                    loadData = true;
                }

                //ha még mindíg nincs meg a regisztráció, akkor egy új létrehozása szükséges
                if (registration == null)
                {
                    registration = CompanyGroup.Domain.RegistrationModule.Factory.CreateRegistration();

                    loadData = true;
                }

                //bankszámlaszámok szétdarabolása
                registration.BankAccountList.ForEach(x => x.SplitBankAccount());

                CompanyGroup.Dto.RegistrationModule.Registration response = new RegistrationToRegistration().MapDomainToDto(registration);

                response.Visitor = new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor);

                response.LoadData = loadData;

                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// regisztráció törölt státuszba állítása
        /// </summary>
        /// <param name="id"></param>
        public CompanyGroup.Dto.ServiceResponse.Empty Remove(string id)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(id), "Registration id cannot be null or empty!");

                registrationRepository.Remove(id);

                return new CompanyGroup.Dto.ServiceResponse.Empty();
            }
            catch (Exception ex)
            {
                throw (ex);
            }            
        }



        /// <summary>
        /// regisztráció elküldése
        /// </summary>
        /// <param name="id"></param>
        public CompanyGroup.Dto.ServiceResponse.PostRegistration Post(CompanyGroup.Dto.ServiceRequest.PostRegistration request)
        {
            try
            {
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.RegistrationId), "Registration id cannot be null or empty!");

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                //ha be van jelentkezve, akkor le kell kérdezni a szerződés adatait azért, hogy el lehessen dönteni kis / vagy módosításról van-e szó?
                int registrationMethod = 1;

                if (visitor.IsValidLogin)
                {
                    CompanyGroup.Domain.PartnerModule.CustomerContractData contractData = customerRepository.GetCustomerContractData(visitor.CustomerId);

                    registrationMethod = contractData.CalculateRegistrationMethod(registration);
                }

                //vevő felvitel
                CompanyGroup.Domain.RegistrationModule.CustomerCreate customer = new CompanyGroup.Domain.RegistrationModule.CustomerCreate()
                {
                    CompanyCertificateFile = "",
                    CustomerId = visitor.CustomerId,
                    CustomerName = registration.CompanyData.CustomerName,
                    DataAreaId = "hrp", //registration.DataAreaId,
                    InvoiceCity = registration.InvoiceAddress.City,
                    InvoiceCountry = registration.InvoiceAddress.Country,
                    InvoiceCounty = "", //registration.InvoiceAddress.County,
                    InvoiceEmail = registration.CompanyData.MainEmail,
                    InvoiceFax = "",
                    InvoicePhone = registration.InvoiceAddress.Phone,
                    InvoicePostCode = registration.InvoiceAddress.ZipCode,
                    InvoiceStreet = registration.InvoiceAddress.Street,
                    Method = registrationMethod,              // 1: új, 2: módosít, 3: töröl, 4: kis módosítás
                    RecId = 0,
                    RegEmail = registration.DataRecording.Email,
                    RegName = registration.DataRecording.Name,
                    RegNumber = "", //registration.DataRecording.Number,
                    RegPhone = registration.DataRecording.Phone,
                    NewsletterSubScription = registration.CompanyData.NewsletterToMainEmail ? 1 : 0,
                    SignatureEntityFile = registration.CompanyData.SignatureEntityFile,
                    VatNumber = registration.CompanyData.VatNumber,
                    EUVatNumber = registration.CompanyData.EUVatNumber,

                    /*
                             webAdministrator.setAllowOrder( str2int(xmlReader.readElementString2('AllowOrder')) );
                            webAdministrator.setAllowReceiptOfGoods( str2int(xmlReader.readElementString2('AllowReceiptOfGoods')) );
                            webAdministrator.setCellularPhone( xmlReader.readElementString2('CellularPhone') );
                            webAdministrator.setContactPersonId( xmlReader.readElementString2('ContactPersonId') );
                            webAdministrator.setDataAreaId( xmlReader.readElementString2('DataAreaId') );
                            webAdministrator.setDirector( str2int(xmlReader.readElementString2('Director')) );
                            webAdministrator.setEmail( xmlReader.readElementString2('Email') );
                            webAdministrator.setEmailArriveOfGoods( str2int(xmlReader.readElementString2('EmailArriveOfGoods')) );
                            webAdministrator.setEmailOfDelivery( str2int(xmlReader.readElementString2('EmailOfDelivery')) );
                            webAdministrator.setEmailOfOrderConfirm( str2int(xmlReader.readElementString2('EmailOfOrderConfirm')) );
                            webAdministrator.setFax( xmlReader.readElementString2('Fax') );
                            webAdministrator.setFinanceManager( str2int(xmlReader.readElementString2('FinanceManager')) );
                            webAdministrator.setFirstName( xmlReader.readElementString2('FirstName') );
                            webAdministrator.setFunctionId( xmlReader.readElementString2('FunctionId') );
                            webAdministrator.setGender( str2int(xmlReader.readElementString2('Gender')) );
                            webAdministrator.setInvoiceInfo( str2int(xmlReader.readElementString2('InvoiceInfo')) );
                            webAdministrator.setLastName( xmlReader.readElementString2('LastName') );
                            webAdministrator.setLeftCompany( str2int(xmlReader.readElementString2('LeftCompany')) );
                            webAdministrator.setMethod( str2int(xmlReader.readElementString2('Method')) );
                            webAdministrator.setNewsletter( str2int(xmlReader.readElementString2('Newsletter')) );
                            webAdministrator.setPhone( xmlReader.readElementString2('Phone') );
                            webAdministrator.setPhoneLocal( xmlReader.readElementString2('PhoneLocal') );
                            webAdministrator.setPriceListDownload( str2int(xmlReader.readElementString2('PriceListDownload')) );
                            webAdministrator.setRecId( str2int64(xmlReader.readElementString2('RecId')) );
                            webAdministrator.setRefRecId( str2int64(xmlReader.readElementString2('RefRecId')) );
                            webAdministrator.setSalesManager( str2int(xmlReader.readElementString2('SalesManager')) );
                            webAdministrator.setSimpleContact( str2int(xmlReader.readElementString2('SimpleContact')) );
                            webAdministrator.setSimpleSales( str2int(xmlReader.readElementString2('SimpleSales')) );
                            webAdministrator.setWebAdmin( str2int(xmlReader.readElementString2('WebAdmin')) );
                            webAdministrator.setWebLoginName( xmlReader.readElementString2('WebLoginName') );
                            webAdministrator.setWebPassword( xmlReader.readElementString2('WebPassword') );
                     * 
                          <option value="Igazgató">Igazgató</option>
						  <option value="Pénzügyi vezető">Pénzügyi vezető</option>
						  <option value="Kereskedelmi vezető">Kereskedelmi vezető</option>
						  <option value="Kapcsolattartó">Kapcsolattartó</option>
						  <option value="Kereskedő">Kereskedő</option>
                     */

                    WebAdministrator = new Domain.RegistrationModule.ContactPersonCreate()
                    {
                        AllowOrder = registration.WebAdministrator.AllowOrder ? 1 : 0,
                        AllowReceiptOfGoods = registration.WebAdministrator.AllowReceiptOfGoods ? 1 : 0,
                        CellularPhone = registration.WebAdministrator.Telephone,
                        ContactPersonId = registration.WebAdministrator.ContactPersonId,
                        DataAreaId = "hrp",
                        Director = registration.WebAdministrator.Positions.Contains("Igazgató") ? 1 : 0,
                        Email = registration.WebAdministrator.Email,
                        EmailArriveOfGoods = registration.WebAdministrator.EmailArriveOfGoods ? 1 : 0,
                        EmailOfDelivery = registration.WebAdministrator.EmailOfDelivery ? 1 : 0,
                        EmailOfOrderConfirm = registration.WebAdministrator.EmailOfOrderConfirm ? 1 : 0,
                        Fax = "",
                        FinanceManager = registration.WebAdministrator.Positions.Contains("Pénzügyi vezető") ? 1 : 0,
                        FirstName = registration.WebAdministrator.FirstName,
                        FunctionId = "",
                        Gender = 1,
                        InvoiceInfo = registration.WebAdministrator.InvoiceInfo ? 1 : 0,
                        LastName = registration.WebAdministrator.LastName,
                        LeftCompany = 0,
                        Method = registrationMethod, 
                        Newsletter = registration.WebAdministrator.Newsletter ? 1 : 0,
                        Phone = registration.WebAdministrator.Telephone,
                        PhoneLocal = "",
                        PriceListDownload = registration.WebAdministrator.PriceListDownload ? 1 : 0,
                        RecId = 0,
                        RefRecId = 0,   //custTable RecId
                        SalesManager = registration.WebAdministrator.Positions.Contains("Kereskedelmi vezető") ? 1 : 0,
                        SimpleContact = registration.WebAdministrator.Positions.Contains("Kapcsolattartó") ? 1 : 0,
                        SimpleSales = registration.WebAdministrator.Positions.Contains("Kereskedő") ? 1 : 0,
                        WebAdmin = 1,
                        WebLoginName = registration.WebAdministrator.UserName,
                        WebPassword = registration.WebAdministrator.Password
                    },
                    MailAddress = new CompanyGroup.Domain.RegistrationModule.MailAddressCreate()
                    {
                        City = registration.MailAddress.City,
                        Country = registration.MailAddress.Country,
                        County = "",
                        PostCode = registration.MailAddress.ZipCode,
                        Street = registration.MailAddress.Street
                    }
                };

                CompanyGroup.Domain.RegistrationModule.CustomerCreateResult customerCreateResult = this.customerRepository.Create(customer);

                //ha nem sikerült a regisztráció cégadat, webadmin és levelezési cím része, akkor nem lehet tovább menni
                if (customerCreateResult.RecId.Equals(0))
                {
                    return new CompanyGroup.Dto.ServiceResponse.PostRegistration(false, "A regisztráció cégadat, webadmin és levelezési cím rögzítésekor hiba történt!", customerCreateResult.RegId);
                }

                //szállítási címek felvitele
                registration.DeliveryAddressList.ForEach(delegate(CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddr)
                {
                    CompanyGroup.Domain.RegistrationModule.DeliveryAddressCreate deliveryAddress = new CompanyGroup.Domain.RegistrationModule.DeliveryAddressCreate()
                    {
                        AddressId = "",
                        City = deliveryAddr.City,
                        Country = deliveryAddr.CountryRegionId,
                        County = "",
                        DataAreaId = "hrp",
                        PostCode = deliveryAddr.ZipCode,
                        RecId = deliveryAddr.RecId,
                        RefRecId = customerCreateResult.RecId,
                        Street = deliveryAddr.Street
                    };

                    long deliveryAddressResult = customerRepository.CreateDeliveryAddress(deliveryAddress);
                    //string deliveryAddrXml = this.Serialize<Shared.Web.Dynamics.Entities.DeliveryAddrReg>(TranslateDeliveryAddrToDeliveryAddrReg(result.RecId, deliveryAddr));

                    //object resultCreateDeliveryAddr = dynamics.CallMethod("createDeliveryAddress", deliveryAddrXml);

                    //long deliveryAddrRecId = 0;

                    //long.TryParse(resultCreateDeliveryAddr.ToString(), out deliveryAddrRecId);
                });

                //kapcsolattartók felvitele 
                registration.ContactPersonList.ForEach(delegate(CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson)
                {
                    CompanyGroup.Domain.RegistrationModule.ContactPersonCreate contactPersonCreate = new Domain.RegistrationModule.ContactPersonCreate()
                    {
                        AllowOrder = contactPerson.AllowOrder ? 1 : 0,
                        AllowReceiptOfGoods = contactPerson.AllowReceiptOfGoods ? 1 : 0,
                        CellularPhone = contactPerson.Telephone,
                        ContactPersonId = contactPerson.ContactPersonId,
                        DataAreaId = "hrp",
                        Director = contactPerson.Positions.Contains("Igazgató") ? 1 : 0,
                        Email = contactPerson.Email,
                        EmailArriveOfGoods = contactPerson.EmailArriveOfGoods ? 1 : 0,
                        EmailOfDelivery = contactPerson.EmailOfDelivery ? 1 : 0,
                        EmailOfOrderConfirm = contactPerson.EmailOfOrderConfirm ? 1 : 0,
                        Fax = "",
                        FinanceManager = contactPerson.Positions.Contains("Pénzügyi vezető") ? 1 : 0,
                        FirstName = contactPerson.FirstName,
                        FunctionId = "",
                        Gender = 1,
                        InvoiceInfo = contactPerson.InvoiceInfo ? 1 : 0,
                        LastName = contactPerson.LastName,
                        LeftCompany = 0,
                        Method = visitor.IsValidLogin ? 2 : 1,
                        Newsletter = contactPerson.Newsletter ? 1 : 0,
                        Phone = contactPerson.Telephone,
                        PhoneLocal = "",
                        PriceListDownload = contactPerson.PriceListDownload ? 1 : 0,
                        RecId = contactPerson.RecId,
                        RefRecId = customerCreateResult.RecId,
                        SalesManager = contactPerson.Positions.Contains("Kereskedelmi vezető") ? 1 : 0,
                        SimpleContact = contactPerson.Positions.Contains("Kapcsolattartó") ? 1 : 0,
                        SimpleSales = contactPerson.Positions.Contains("Kereskedő") ? 1 : 0,
                        WebAdmin = contactPerson.WebAdmin ? 1 : 0,
                        WebLoginName = contactPerson.UserName,
                        WebPassword = contactPerson.Password
                    };

                    long contactPersonResult = customerRepository.CreateContactPerson(contactPersonCreate);

                    //string contactPersonXml = this.Serialize<Shared.Web.Dynamics.Entities.ContactPersonReg>(TranslateCpRegDataToContactPersonReg(result.RecId, method, contactPerson, request.DataAreaId));

                    //object resultCreateContactPerson = dynamics.CallMethod("createContactPerson", contactPersonXml);

                    //long contactPersonRecId = 0;

                    //long.TryParse(resultCreateContactPerson.ToString(), out contactPersonRecId);
                });
                //bankszámlaszámok felvitele 
                registration.BankAccountList.ForEach(delegate(CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount)
                {
                    CompanyGroup.Domain.RegistrationModule.BankAccountCreate bankAccountCreate = new CompanyGroup.Domain.RegistrationModule.BankAccountCreate()
                    {
                        AccountNumber = bankAccount.Number,
                        DataAreaId = "hrp",
                        Method = registrationMethod,
                        RecId = bankAccount.RecId,
                        RefRecId = customerCreateResult.RecId
                    };

                    long bankAccountResult = customerRepository.CreateBankAccount(bankAccountCreate);

                    //string bankAccountXml = this.Serialize<Shared.Web.Dynamics.Entities.BankAccountReg>(TranslateBankAccountToBankAccountReg(result.RecId, method, bankAccount, request.DataAreaId));

                    //object resultCreateBankAccount = dynamics.CallMethod("createBankAccount", bankAccountXml);

                    //long bankAccountRecId = 0;

                    //long.TryParse(resultCreateBankAccount.ToString(), out bankAccountRecId);
                });

                #region "regisztrációs file létrehozása, template-ből generált tartalommal"

                CompanyGroup.Data.RegistrationModule.RegistrationFileRepository registrationFileRepository = new CompanyGroup.Data.RegistrationModule.RegistrationFileRepository(registration); 

                string registrationHtml = registrationFileRepository.ReadRegistrationHtmlTemplate(String.Format("{0}{1}", RegistrationService.RegistrationTemplateFilePath, RegistrationService.RegistrationTemplateFileName));

                string htmlContent = registrationFileRepository.RenderRegistrationDataToHtml(customerCreateResult.RegId, registrationHtml);

                string registrationFileNameWithPath = System.IO.Path.Combine(RegistrationService.RegistrationFilePath, String.Format("{0}.html", customerCreateResult.RecId));

                registrationFileRepository.CreateRegistrationFile(registrationFileNameWithPath, htmlContent);

                #endregion

                //vissza kell írni a generált file nevét az ideiglenes regisztrációs lapra
                CompanyGroup.Domain.RegistrationModule.CustomerRegistrationPrintedFile customerRegistrationPrintedFileRequest = new Domain.RegistrationModule.CustomerRegistrationPrintedFile() 
                                                                                                                                    { 
                                                                                                                                        DataAreaId = customerCreateResult.DataAreaId, 
                                                                                                                                        FileName = String.Format("{0}.html", customerCreateResult.RecId), 
                                                                                                                                        RecId = customerCreateResult.RecId
                                                                                                                                    };

                CompanyGroup.Domain.RegistrationModule.CustomerRegistrationPrintedFileResult customerRegistrationPrintedFileResult = customerRepository.UpdateCustomerRegistrationPrintedFile(customerRegistrationPrintedFileRequest);

                //sikeres ERP rögzítés után
                registrationRepository.Post(request.RegistrationId);

                return new CompanyGroup.Dto.ServiceResponse.PostRegistration(true, String.Empty, customerCreateResult.RegId);
            }
            catch(Exception ex)
            {
                throw ex;
                //return new CompanyGroup.Dto.ServiceResponse.PostRegistration(false, ex.Message, String.Empty);
            }
        }

        #region 

        /// <summary>
        /// regisztráció elkészítése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //private Shared.Web.Service.Entities.CustomerResultMsg CreateCustomerRegistration(CreateCustomerRegistration request)
        //{
        //    Shared.Web.Helpers.DynamicsConnector dynamics = null;
        //    try
        //    {
        //        Shared.Web.Service.Entities.LoginInfoData loginInfoData = this.GetLoginInfoData(request.ObjectId, db);

        //        request.CustomerId = loginInfoData.LoginInfoItem.CustAccount;

        //        int method = (loginInfoData.LoginInfoItem.LoggedIn) ? 2 : 1;

        //        if (request.BankAccountList == null || request.BankAccountList.Count == 0) { return ConstructCustomerResultMsg(-5, request.LangId, 0, String.Empty); }

        //        Shared.Web.Dynamics.Entities.CustomerReg reg = new Shared.Web.Dynamics.Entities.CustomerReg()
        //        {
        //            CompanyCertificateFile = request.CompanyCertificateFile,
        //            CustomerId = request.CustomerId,
        //            CustomerName = request.CustomerName,
        //            DataAreaId = request.DataAreaId,
        //            InvoiceCity = request.InvoiceCity,
        //            InvoiceCountry = request.InvoiceCountry,
        //            InvoiceCounty = request.InvoiceCounty,
        //            InvoiceEmail = request.InvoiceEmail,
        //            InvoiceFax = request.InvoiceFax,
        //            InvoicePhone = request.InvoicePhone,
        //            InvoicePostCode = request.InvoicePostCode,
        //            InvoiceStreet = request.InvoiceStreet,
        //            Method = method,
        //            NewsletterSubScription = request.NewsletterSubScription ? 1 : 0,
        //            RecId = 0,
        //            RegEmail = request.RegEmail,
        //            RegName = request.RegName,
        //            RegNumber = request.RegNumber,
        //            RegPhone = request.RegPhone,
        //            SignatureEntityFile = request.SignatureEntityFile,
        //            VatNumber = request.VatNumber,
        //            WebAdministrator = new Dynamics.Entities.ContactPersonReg()
        //            {
        //                AllowOrder = request.WebAdministrator.AllowOrder ? 1 : 0,
        //                AllowReceiptOfGoods = request.WebAdministrator.AllowReceiptOfGoods ? 1 : 0,
        //                CellularPhone = request.WebAdministrator.CellularPhone,
        //                ContactPersonId = request.WebAdministrator.ContactPersonId,
        //                DataAreaId = request.DataAreaId,
        //                Director = request.WebAdministrator.Director ? 1 : 0,
        //                Email = request.WebAdministrator.Email,
        //                EmailArriveOfGoods = request.WebAdministrator.EmailArriveOfGoods ? 1 : 0,
        //                EmailOfDelivery = request.WebAdministrator.EmailOfDelivery ? 1 : 0,
        //                EmailOfOrderConfirm = request.WebAdministrator.EmailOfOrderConfirm ? 1 : 0,
        //                Fax = request.WebAdministrator.Fax,
        //                FinanceManager = request.WebAdministrator.FinanceManager ? 1 : 0,
        //                FirstName = request.WebAdministrator.FirstName,
        //                FunctionId = request.WebAdministrator.FunctionId,
        //                Gender = request.WebAdministrator.Gender,
        //                InvoiceInfo = request.WebAdministrator.InvoiceInfo ? 1 : 0,
        //                LastName = request.WebAdministrator.LastName,
        //                LeftCompany = 0,
        //                Newsletter = request.WebAdministrator.Newsletter ? 1 : 0,
        //                Phone = request.WebAdministrator.Phone,
        //                PhoneLocal = request.WebAdministrator.PhoneLocal,
        //                PriceListDownload = request.WebAdministrator.PriceListDownload ? 1 : 0,
        //                RecId = 0,
        //                RefRecId = 0,   //custTable RecId
        //                SalesManager = request.WebAdministrator.SalesManager ? 1 : 0,
        //                SimpleContact = request.WebAdministrator.SimpleContact ? 1 : 0,
        //                SimpleSales = request.WebAdministrator.SimpleSales ? 1 : 0,
        //                WebAdmin = 1,
        //                WebLoginName = request.WebAdministrator.WebLoginName,
        //                WebPassword = request.WebAdministrator.WebPassword
        //            },
        //            MailAddress = new Dynamics.Entities.MailAddrReg()
        //            {
        //                City = request.MailCity,
        //                Country = request.MailCity,
        //                County = request.MailCounty,
        //                PostCode = request.MailPostcode,
        //                Street = request.MailStreet
        //            }
        //        };

        //        string customerRegXml = this.Serialize<Shared.Web.Dynamics.Entities.CustomerReg>(reg);

        //        dynamics = new Shared.Web.Helpers.DynamicsConnector(CreateCustomerRegistrationService.UserName,
        //                                                            CreateCustomerRegistrationService.Password,
        //                                                            CreateCustomerRegistrationService.Domain,
        //                                                            CreateCustomerRegistrationService.DataAreaId,
        //                                                            CreateCustomerRegistrationService.Language,
        //                                                            CreateCustomerRegistrationService.ObjectServer,
        //                                                            CreateCustomerRegistrationService.ClassName);

        //        dynamics.Connect();

        //        object customerRegResult = dynamics.CallMethod("createCustomer", customerRegXml);

        //        Shared.Web.Dynamics.Entities.CustomerRegResult result = CreateCustomerRegResult(customerRegResult.ToString());

        //        if (result.RecId > 0)
        //        {
        //            //szállítási címek felvitele
        //            request.DeliveryAddressList.ForEach(delegate(Shared.Web.Service.Entities.DeliveryAddr deliveryAddr)
        //            {
        //                string deliveryAddrXml = this.Serialize<Shared.Web.Dynamics.Entities.DeliveryAddrReg>(TranslateDeliveryAddrToDeliveryAddrReg(result.RecId, deliveryAddr));

        //                object resultCreateDeliveryAddr = dynamics.CallMethod("createDeliveryAddress", deliveryAddrXml);

        //                long deliveryAddrRecId = 0;

        //                long.TryParse(resultCreateDeliveryAddr.ToString(), out deliveryAddrRecId);
        //            });

        //            //kapcsolattartók felvitele 
        //            request.ContactPersonItems.ForEach(delegate(Shared.Web.Service.Entities.CpRegData contactPerson)
        //            {
        //                string contactPersonXml = this.Serialize<Shared.Web.Dynamics.Entities.ContactPersonReg>(TranslateCpRegDataToContactPersonReg(result.RecId, method, contactPerson, request.DataAreaId));

        //                object resultCreateContactPerson = dynamics.CallMethod("createContactPerson", contactPersonXml);

        //                long contactPersonRecId = 0;

        //                long.TryParse(resultCreateContactPerson.ToString(), out contactPersonRecId);
        //            });
        //            //bankszámlaszámok felvitele 
        //            request.BankAccountList.ForEach(delegate(Shared.Web.Service.Entities.CustomerBankAccountItem bankAccount)
        //            {
        //                string bankAccountXml = this.Serialize<Shared.Web.Dynamics.Entities.BankAccountReg>(TranslateBankAccountToBankAccountReg(result.RecId, method, bankAccount, request.DataAreaId));

        //                object resultCreateBankAccount = dynamics.CallMethod("createBankAccount", bankAccountXml);

        //                long bankAccountRecId = 0;

        //                long.TryParse(resultCreateBankAccount.ToString(), out bankAccountRecId);
        //            });

        //            //szerződés file generálása
        //            if (CreateRegistrationFile(result.RecId, reg, request.BankAccountList, request.DeliveryAddressList, request.ContactPersonItems))
        //            {
        //                object resultCustRegFile = dynamics.CallMethod("setCustomerRegistrationFile", result.RecId);

        //                int custRegFileRet = 0;

        //                int.TryParse(resultCustRegFile.ToString(), out custRegFileRet);
        //            }
        //        }
        //        return (result.RecId == 0) ? ConstructCustomerResultMsg(-4, "registration failed", 0, String.Empty) : ConstructCustomerResultMsg(1, "registration completed", result.RecId, result.RegId);
        //    }
        //    catch
        //    {
        //        return ConstructCustomerResultMsg(-6, "registration failed", 0, String.Empty);
        //    }
        //    finally
        //    {
        //        if (dynamics != null)
        //        {
        //            dynamics.Disconnect();
        //        }
        //    }
        //}

        //private Shared.Web.Service.Entities.CustomerResultMsg ConstructCustomerResultMsg(int code, string msg, long recId, string regId)
        //{
        //    return new Shared.Web.Service.Entities.CustomerResultMsg() { Code = code, Msg = msg, RecId = recId, RegId = regId };
        //}

        //private Shared.Web.Dynamics.Entities.CustomerRegResult CreateCustomerRegResult(string xml)
        //{
        //    if (String.IsNullOrEmpty(xml))
        //    {
        //        return new Shared.Web.Dynamics.Entities.CustomerRegResult();
        //    }
        //    return this.DeSerialize<Shared.Web.Dynamics.Entities.CustomerRegResult>(xml);
        //}

        #endregion



        /// <summary>
        /// adatlapot kitöltő adatainak módosítása
        /// </summary>
        /// <param name="dataRecording"></param>
        public CompanyGroup.Dto.ServiceResponse.UpdateDataRecording UpdateDataRecording(CompanyGroup.Dto.ServiceRequest.UpdateDataRecording request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration updateDataRecording request cannot be null or empty!");

                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Domain.RegistrationModule.DataRecording dataRecording = new DataRecordingToDataRecording().MapDtoToDomain(request.DataRecording);

                registrationRepository.UpdateDataRecording(request.RegistrationId, dataRecording);

                return new CompanyGroup.Dto.ServiceResponse.UpdateDataRecording(true, String.Empty, new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor));
            }
            catch (Exception ex)
            {
                throw ex; 
                //return new CompanyGroup.Dto.ServiceResponse.UpdateDataRecording(false, ex.Message, new CompanyGroup.Dto.PartnerModule.Visitor());
            }
        }

        /// <summary>
        /// regisztrációs adatok mint - cég, számla, levelezési adatok felvitele, módosítása 
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData UpdateRegistrationData(CompanyGroup.Dto.ServiceRequest.UpdateRegistrationData request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration updateRegistrationData request cannot be null or empty!");

                request.CompanyData.CustomerId = request.CompanyData.CustomerId ?? String.Empty;

                request.CompanyData.EUVatNumber = request.CompanyData.EUVatNumber ?? String.Empty;

                request.CompanyData.SignatureEntityFile = request.CompanyData.SignatureEntityFile ?? String.Empty;

                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Domain.RegistrationModule.InvoiceAddress invoiceAddress = new InvoiceAddressToInvoiceAddress().MapDtoToDomain(request.InvoiceAddress);

                CompanyGroup.Domain.RegistrationModule.CompanyData companyData = new CompanyDataToCompanyData().MapDtoToDomain(request.CompanyData);

                CompanyGroup.Domain.RegistrationModule.MailAddress mailAddress = null;

                //beállította a levelezési cím nem egyezik a számlázási címmel kapcsolót?
                if (request.ModifyMailAddress)
                {
                    //nem egyezés beállítás esetén a megadottakat kell kiolvasni
                    mailAddress = new MailAddressToMailAddress().MapDtoToDomain(request.MailAddress);
                }
                else
                {
                    //egyezés beállítás esetén a számlázási cím lesz a levelezési cím 
                    mailAddress = new Domain.RegistrationModule.MailAddress(request.InvoiceAddress.City, request.InvoiceAddress.CountryRegionId, request.InvoiceAddress.ZipCode, request.InvoiceAddress.Street);
                }

                //beállította a szállítási cím nem egyezik a számlázási címmel kapcsolót, ezért a számlázási címet mint szállítási címet hozzá kell adni
                if (request.ModifyDeliveryAddress)
                {
                    //ha nem adta hozzá, de kitöltötte a szállítási címet az űrlapmezők között, akkor hozzáadásra kerül
                    if (!String.IsNullOrEmpty(request.DeliveryAddress.City) && !String.IsNullOrEmpty(request.DeliveryAddress.CountryRegionId) && !String.IsNullOrEmpty(request.DeliveryAddress.Street) && !String.IsNullOrEmpty(request.DeliveryAddress.ZipCode))
                    {
                        CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress = new CompanyGroup.Domain.RegistrationModule.DeliveryAddress(0, request.DeliveryAddress.City, request.DeliveryAddress.ZipCode, request.DeliveryAddress.Street, request.DeliveryAddress.CountryRegionId);

                        registrationRepository.AddDeliveryAddress(request.RegistrationId, deliveryAddress);
                    }
                }
                else
                {
                    CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress = new CompanyGroup.Domain.RegistrationModule.DeliveryAddress(0, request.InvoiceAddress.City, request.InvoiceAddress.ZipCode, request.InvoiceAddress.Street, request.InvoiceAddress.CountryRegionId);

                    registrationRepository.AddDeliveryAddress(request.RegistrationId, deliveryAddress);
                }
                //ha nem adta hozzá, de kitöltötte a bankszámlaszámot az űrlapmezők között, akkor hozzáadásra kerül
                if (!String.IsNullOrEmpty(request.BankAccount.Part1) && !String.IsNullOrEmpty(request.BankAccount.Part2) && !String.IsNullOrEmpty(request.BankAccount.Part3))
                { 
                    CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount = new CompanyGroup.Domain.RegistrationModule.BankAccount(request.BankAccount.Part1, request.BankAccount.Part2, request.BankAccount.Part3, 0);

                    registrationRepository.AddBankAccount(request.RegistrationId, bankAccount);
                }

                registrationRepository.UpdateRegistrationData(request.RegistrationId, companyData, invoiceAddress, mailAddress);

                return new CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData() { Message = "", Successed = true, Visitor = new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor) };
            }
            catch (Exception ex)
            {
                throw ex;
                //return new CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData() { Message = ex.Message, Successed = false, Visitor = new CompanyGroup.Dto.PartnerModule.Visitor() };
            }
        }

        /// <summary>
        /// webadmin adatok módosítása
        /// </summary>
        /// <param name="id"></param>
        /// <param name="webAdministrator"></param>
        public CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator UpdateWebAdministrator(CompanyGroup.Dto.ServiceRequest.UpdateWebAdministrator request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration updateWebAdministrator request cannot be null or empty!");

                request.WebAdministrator.ContactPersonId = request.WebAdministrator.ContactPersonId ?? String.Empty;

                if (request.WebAdministrator.Positions == null)
                {
                    request.WebAdministrator.Positions = new List<string>();
                }

                //üres listaelemek eltávolítása
                request.WebAdministrator.Positions.RemoveAll(x => String.IsNullOrWhiteSpace(x));

                //látogató adatainak kiolvasása
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Domain.RegistrationModule.WebAdministrator webAdministrator = new WebAdministratorToWebAdministrator().MapDtoToDomain(request.WebAdministrator);

                registrationRepository.UpdateWebAdministrator(request.RegistrationId, webAdministrator);

                return new CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator() { Message = "", Succeeded = true, Visitor = new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor) };
            }
            catch (Exception ex)
            {
                throw ex;
                //return new CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator() { Message = ex.Message, Succeeded = false, Visitor = new CompanyGroup.Dto.PartnerModule.Visitor() };
            }
        }

        #region "szállítási címek"

        /// <summary>
        /// szállítási címek kiolvasása GetDeliveryAddress
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses GetDeliveryAddresses(CompanyGroup.Dto.ServiceRequest.GetDeliveryAddress request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addDeliveryAddress request cannot be null or empty!");

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> deliveryAddressList = registration.DeliveryAddressList.ConvertAll(x => new DeliveryAddressToDeliveryAddress().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses();

                response.Items.AddRange(deliveryAddressList);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// szállítási cím hozzáadása
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses AddDeliveryAddress(CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addDeliveryAddress request cannot be null or empty!");

                request.DeliveryAddress.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

                CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress = new DeliveryAddressToDeliveryAddress().MapDtoToDomain(request.DeliveryAddress);

                registrationRepository.AddDeliveryAddress(request.RegistrationId, deliveryAddress);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> deliveryAddressList = registration.DeliveryAddressList.ConvertAll(x => new DeliveryAddressToDeliveryAddress().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses();

                response.Items.AddRange(deliveryAddressList);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// szállítási cím adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses UpdateDeliveryAddress(CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration updateDeliveryAddress request cannot be null or empty!");

                CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress = new DeliveryAddressToDeliveryAddress().MapDtoToDomain(request.DeliveryAddress);

                registrationRepository.UpdateDeliveryAddress(request.RegistrationId, deliveryAddress);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> deliveryAddressList = registration.DeliveryAddressList.ConvertAll(x => new DeliveryAddressToDeliveryAddress().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses();

                response.Items.AddRange(deliveryAddressList);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// szállítási cím törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses RemoveDeliveryAddress(CompanyGroup.Dto.ServiceRequest.RemoveDeliveryAddress request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.RegistrationId), "Registration id cannot be null or empty!");
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.DeliveryAddressId), "deliveryAddress id cannot be null or empty!");

                registrationRepository.RemoveDeliveryAddress(request.RegistrationId, request.DeliveryAddressId);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> deliveryAddressList = registration.DeliveryAddressList.ConvertAll(x => new DeliveryAddressToDeliveryAddress().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses();

                response.Items.AddRange(deliveryAddressList);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region "bankszámlaszám"

        /// <summary>
        /// bankszámlaszám lista
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.RegistrationModule.BankAccounts GetBankAccounts(CompanyGroup.Dto.ServiceRequest.GetBankAccounts request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addBankAccount request cannot be null or empty!");

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                registration.BankAccountList.ForEach(x => x.SplitBankAccount());

                List<CompanyGroup.Dto.RegistrationModule.BankAccount> bankAccountList = registration.BankAccountList.ConvertAll(x => new BankAccountToBankAccount().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.BankAccounts response = new CompanyGroup.Dto.RegistrationModule.BankAccounts();

                response.Items.AddRange(bankAccountList);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// bankszámlaszám felvitele
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.RegistrationModule.BankAccounts AddBankAccount(CompanyGroup.Dto.ServiceRequest.AddBankAccount request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addBankAccount request cannot be null or empty!");

                request.BankAccount.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

                CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount = new BankAccountToBankAccount().MapDtoToDomain(request.BankAccount);

                registrationRepository.AddBankAccount(request.RegistrationId, bankAccount);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                registration.BankAccountList.ForEach(x => x.SplitBankAccount());

                List<CompanyGroup.Dto.RegistrationModule.BankAccount> bankAccountList = registration.BankAccountList.ConvertAll(x => new BankAccountToBankAccount().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.BankAccounts response = new CompanyGroup.Dto.RegistrationModule.BankAccounts();

                response.Items.AddRange(bankAccountList);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// bankszámlaszám adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.BankAccounts UpdateBankAccount(CompanyGroup.Dto.ServiceRequest.UpdateBankAccount request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration updateDeliveryAddress request cannot be null or empty!");

                CompanyGroup.Domain.RegistrationModule.BankAccount bankAccount = new BankAccountToBankAccount().MapDtoToDomain(request.BankAccount);

                registrationRepository.UpdateBankAccount(request.RegistrationId, bankAccount);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                registration.BankAccountList.ForEach(x => x.SplitBankAccount());

                List<CompanyGroup.Dto.RegistrationModule.BankAccount> bankAccountList = registration.BankAccountList.ConvertAll(x => new BankAccountToBankAccount().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.BankAccounts response = new CompanyGroup.Dto.RegistrationModule.BankAccounts();

                response.Items.AddRange(bankAccountList);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// bankszámlaszám törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.BankAccounts RemoveBankAccount(CompanyGroup.Dto.ServiceRequest.RemoveBankAccount request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.RegistrationId), "Registration id cannot be null or empty!");
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.BankAccountId), "BankAccountId id cannot be null or empty!");

                registrationRepository.RemoveBankAccount(request.RegistrationId, request.BankAccountId);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                registration.BankAccountList.ForEach(x => x.SplitBankAccount());

                List<CompanyGroup.Dto.RegistrationModule.BankAccount> bankAccounts = registration.BankAccountList.ConvertAll(x => new BankAccountToBankAccount().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.BankAccounts response = new CompanyGroup.Dto.RegistrationModule.BankAccounts();

                response.Items.AddRange(bankAccounts);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region "kapcsolattartó"

        /// <summary>
        /// kapcsolattartó hozzáadása
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons AddContactPerson(CompanyGroup.Dto.ServiceRequest.AddContactPerson request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addBankAccount request cannot be null or empty!");

                request.ContactPerson.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

                request.ContactPerson.ContactPersonId = request.ContactPerson.ContactPersonId ?? String.Empty;

                if (request.ContactPerson.Positions == null)
                {
                    request.ContactPerson.Positions = new List<string>();
                }

                CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson = new ContactPersonToContactPerson().MapDtoToDomain(request.ContactPerson);

                registrationRepository.AddContactPerson(request.RegistrationId, contactPerson);

                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

                CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons(contactPersonList, new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// kapcsolattartó mentése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons SaveContactPerson(CompanyGroup.Dto.ServiceRequest.SaveContactPerson request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addBankAccount request cannot be null or empty!");

                request.ContactPerson.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

                request.ContactPerson.ContactPersonId = request.ContactPerson.ContactPersonId ?? String.Empty;

                if (request.ContactPerson.Positions == null)
                {
                    request.ContactPerson.Positions = new List<string>();
                }

                //csak akkor kell hozzáadni a kapcsolattartót, ha a kötelező mezők ki vannak töltve
                if (!String.IsNullOrEmpty(request.ContactPerson.Email) && !String.IsNullOrEmpty(request.ContactPerson.FirstName) && !String.IsNullOrEmpty(request.ContactPerson.LastName)
                    && !String.IsNullOrEmpty(request.ContactPerson.Password) && !String.IsNullOrEmpty(request.ContactPerson.UserName))
                {
                    CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson = new ContactPersonToContactPerson().MapDtoToDomain(request.ContactPerson);

                    registrationRepository.AddContactPerson(request.RegistrationId, contactPerson);
                }
                //látogató kiolvasása 
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                //teljes regisztráció kiolvasása azonosító alapján
                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                //kapcsolattartó konvertálása DTO-ra
                List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

                //kapcsolattartó lista
                CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons(contactPersonList, new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// kapcsolattartó kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons GetContactPersons(CompanyGroup.Dto.ServiceRequest.GetContactPerson request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addBankAccount request cannot be null or empty!");

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

                //látogató kiolvasása 
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons(contactPersonList, new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor));

                response.Items.AddRange(contactPersonList);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// kapcsolattartó adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons UpdateContactPerson(CompanyGroup.Dto.ServiceRequest.UpdateContactPerson request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration updateContactPerson request cannot be null or empty!");

                if (request.ContactPerson.Positions == null)
                {
                    request.ContactPerson.Positions = new List<string>();
                }

                CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson = new ContactPersonToContactPerson().MapDtoToDomain(request.ContactPerson);

                registrationRepository.UpdateContactPerson(request.RegistrationId, contactPerson);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

                //látogató kiolvasása 
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons(contactPersonList, new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Kapcsolattartó törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons RemoveContactPerson(CompanyGroup.Dto.ServiceRequest.RemoveContactPerson request)
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.RegistrationId), "Registration id cannot be null or empty!");

                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Id), "ContactPerson id cannot be null or empty!");

                registrationRepository.RemoveContactPerson(request.RegistrationId, request.Id);

                CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

                List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

                //látogató kiolvasása 
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

                CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons(contactPersonList, new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
