using System;
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

            CompanyGroup.Domain.PartnerModule.Visitor visitor = base.GetVisitor(request.VisitorId);

            response.Visitor = new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor);

            return response;
        }

        /// <summary>
        /// új regisztráció hozzáadása
        /// </summary>
        /// <param name="visitor"></param>
        //public string Add(CompanyGroup.Dto.RegistrationModule.Registration request)
        //{
        //    CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration cannot be null!");

        //    try
        //    {
        //        CompanyGroup.Domain.PartnerModule.Visitor visitor = base.GetVisitor(request.VisitorId);

        //        CompanyGroup.Domain.RegistrationModule.Registration registration = new RegistrationToRegistration().MapDtoToDomain(request);

        //        registration.Id = MongoDB.Bson.ObjectId.GenerateNewId();

        //        registration.CompanyId = visitor.CompanyId;

        //        registration.PersonId = visitor.PersonId;

        //        registrationRepository.Add(registration);

        //        return registration.Id.ToString();
        //    }
        //    catch(Exception ex)
        //    {
        //        throw(ex);
        //    }
        //}

        /// <summary>
        /// új regisztráció hozzáadása 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.Registration AddNew(CompanyGroup.Dto.ServiceRequest.AddNewRegistration request)
        {
            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration datarecording cannot be null!");

            try
            {
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
                                                                                        x.RefRecId); 
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
                                                                                                                webAdmin.RefRecId);
                    
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
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(id), "Registration id cannot be null or empty!");
            
            registrationRepository.Remove(id);

            return new CompanyGroup.Dto.ServiceResponse.Empty();
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
                    Method = visitor.IsValidLogin ? 2 : 1,
                    NewsletterSubScription = registration.CompanyData.NewsletterToMainEmail ? 1 : 0,
                    RecId = 0,
                    RegEmail = registration.DataRecording.Email,
                    RegName = registration.DataRecording.Name,
                    RegNumber = "", //registration.DataRecording.Number,
                    RegPhone = registration.DataRecording.Phone,
                    SignatureEntityFile = registration.CompanyData.SignatureEntityFile,
                    VatNumber = registration.CompanyData.VatNumber,
                    WebAdministrator = new Domain.RegistrationModule.ContactPersonCreate()
                    {
                        AllowOrder = registration.WebAdministrator.AllowOrder ? 1 : 0,
                        AllowReceiptOfGoods = registration.WebAdministrator.AllowReceiptOfGoods ? 1 : 0,
                        CellularPhone = registration.WebAdministrator.Telephone,
                        ContactPersonId = registration.WebAdministrator.ContactPersonId,
                        DataAreaId = "hrp",
                        Director = 0,
                        Email = registration.WebAdministrator.Email,
                        EmailArriveOfGoods = registration.WebAdministrator.EmailArriveOfGoods ? 1 : 0,
                        EmailOfDelivery = registration.WebAdministrator.EmailOfDelivery ? 1 : 0,
                        EmailOfOrderConfirm = registration.WebAdministrator.EmailOfOrderConfirm ? 1 : 0,
                        Fax = "",
                        FinanceManager = 0,
                        FirstName = registration.WebAdministrator.FirstName,
                        FunctionId = "",
                        Gender = 0,
                        InvoiceInfo = registration.WebAdministrator.InvoiceInfo ? 1 : 0,
                        LastName = registration.WebAdministrator.LastName,
                        LeftCompany = 0,
                        Newsletter = registration.WebAdministrator.Newsletter ? 1 : 0,
                        Phone = registration.WebAdministrator.Telephone,
                        PhoneLocal = "",
                        PriceListDownload = registration.WebAdministrator.PriceListDownload ? 1 : 0,
                        RecId = 0,
                        RefRecId = 0,   //custTable RecId
                        SalesManager = 0,
                        SimpleContact = 0,
                        SimpleSales = 0,
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

                //string customerRegXml = this.Serialize<Shared.Web.Dynamics.Entities.CustomerReg>(reg);

                //dynamics = new Shared.Web.Helpers.DynamicsConnector(CreateCustomerRegistrationService.UserName,
                //                                                    CreateCustomerRegistrationService.Password,
                //                                                    CreateCustomerRegistrationService.Domain,
                //                                                    CreateCustomerRegistrationService.DataAreaId,
                //                                                    CreateCustomerRegistrationService.Language,
                //                                                    CreateCustomerRegistrationService.ObjectServer,
                //                                                    CreateCustomerRegistrationService.ClassName);

                //dynamics.Connect();

                //object customerRegResult = dynamics.CallMethod("createCustomer", customerRegXml);

                //Shared.Web.Dynamics.Entities.CustomerRegResult result = CreateCustomerRegResult(customerRegResult.ToString());

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
                        RefRecId = 0,
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
                        Director = 0,
                        Email = contactPerson.Email,
                        EmailArriveOfGoods = contactPerson.EmailArriveOfGoods ? 1 : 0,
                        EmailOfDelivery = contactPerson.EmailOfDelivery ? 1 : 0,
                        EmailOfOrderConfirm = contactPerson.EmailOfOrderConfirm ? 1 : 0,
                        Fax = "",
                        FinanceManager = 0,
                        FirstName = contactPerson.FirstName,
                        FunctionId = "",
                        Gender = 0,
                        InvoiceInfo = contactPerson.InvoiceInfo ? 1 : 0,
                        LastName = contactPerson.LastName,
                        LeftCompany = 0,
                        Method = 1,
                        Newsletter = contactPerson.Newsletter ? 1 : 0,
                        Phone = contactPerson.Telephone,
                        PhoneLocal = "",
                        PriceListDownload = contactPerson.PriceListDownload ? 1 : 0,
                        RecId = contactPerson.RecId,
                        RefRecId = 0,
                        SalesManager = 0,
                        SimpleContact = 0,
                        SimpleSales = 0,
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
                        Method = 1,
                        RecId = bankAccount.RecId,
                        RefRecId = 0
                    };

                    long bankAccountResult = customerRepository.CreateBankAccount(bankAccountCreate);

                    //string bankAccountXml = this.Serialize<Shared.Web.Dynamics.Entities.BankAccountReg>(TranslateBankAccountToBankAccountReg(result.RecId, method, bankAccount, request.DataAreaId));

                    //object resultCreateBankAccount = dynamics.CallMethod("createBankAccount", bankAccountXml);

                    //long bankAccountRecId = 0;

                    //long.TryParse(resultCreateBankAccount.ToString(), out bankAccountRecId);
                });

                //sikeres ERP rögzítés után
                registrationRepository.Post(request.RegistrationId);

                return new CompanyGroup.Dto.ServiceResponse.PostRegistration() { Message = "", Successed = true };
            }
            catch(Exception ex)
            {
                return new CompanyGroup.Dto.ServiceResponse.PostRegistration() { Message = ex.Message, Successed = false };
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

                CompanyGroup.Domain.RegistrationModule.DataRecording dataRecording = new DataRecordingToDataRecording().MapDtoToDomain(request.DataRecording);

                registrationRepository.UpdateDataRecording(request.RegistrationId, dataRecording);

                return new CompanyGroup.Dto.ServiceResponse.UpdateDataRecording() { Message = "", Successed = true };
            }
            catch (Exception ex)
            {
                return new CompanyGroup.Dto.ServiceResponse.UpdateDataRecording() { Message = ex.Message, Successed = false };
            }
        }

        /// <summary>
        /// regisztrációs adatok módosítása 
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

                CompanyGroup.Domain.RegistrationModule.CompanyData companyData = new CompanyDataToCompanyData().MapDtoToDomain(request.CompanyData);

                CompanyGroup.Domain.RegistrationModule.InvoiceAddress invoiceAddress = new InvoiceAddressToInvoiceAddress().MapDtoToDomain(request.InvoiceAddress);

                CompanyGroup.Domain.RegistrationModule.MailAddress mailAddress = new MailAddressToMailAddress().MapDtoToDomain(request.MailAddress);

                registrationRepository.UpdateRegistrationData(request.RegistrationId, companyData, invoiceAddress, mailAddress);

                return new CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData() { Message = "", Successed = true };
            }
            catch (Exception ex)
            {
                return new CompanyGroup.Dto.ServiceResponse.UpdateRegistrationData() { Message = ex.Message, Successed = false };
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

                CompanyGroup.Domain.RegistrationModule.WebAdministrator webAdministrator = new WebAdministratorToWebAdministrator().MapDtoToDomain(request.WebAdministrator);

                registrationRepository.UpdateWebAdministrator(request.RegistrationId, webAdministrator);

                return new CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator() { Message = "", Successed = true };
            }
            catch (Exception ex)
            {
                return new CompanyGroup.Dto.ServiceResponse.UpdateWebAdministrator() { Message = ex.Message, Successed = false };
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
            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addDeliveryAddress request cannot be null or empty!");

            CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

            List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> deliveryAddressList = registration.DeliveryAddressList.ConvertAll(x => new DeliveryAddressToDeliveryAddress().MapDomainToDto(x));

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses();

            response.Items.AddRange(deliveryAddressList);

            return response;
        }

        /// <summary>
        /// szállítási cím hozzáadása
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses AddDeliveryAddress(CompanyGroup.Dto.ServiceRequest.AddDeliveryAddress request)
        {
            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addDeliveryAddress request cannot be null or empty!");

            request.DeliveryAddress.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            CompanyGroup.Domain.RegistrationModule.DeliveryAddress deliveryAddress = new DeliveryAddressToDeliveryAddress().MapDtoToDomain(request.DeliveryAddress);

            registrationRepository.AddDeliveryAddress(request.RegistrationId, deliveryAddress);

            CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

            List<CompanyGroup.Dto.RegistrationModule.DeliveryAddress> deliveryAddressList = registration.DeliveryAddressList.ConvertAll( x => new DeliveryAddressToDeliveryAddress().MapDomainToDto( x ) );

            CompanyGroup.Dto.RegistrationModule.DeliveryAddresses response = new CompanyGroup.Dto.RegistrationModule.DeliveryAddresses();

            response.Items.AddRange(deliveryAddressList);

            return response;
        }

        /// <summary>
        /// szállítási cím adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses UpdateDeliveryAddress(CompanyGroup.Dto.ServiceRequest.UpdateDeliveryAddress request)
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

        /// <summary>
        /// szállítási cím törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.DeliveryAddresses RemoveDeliveryAddress(CompanyGroup.Dto.ServiceRequest.RemoveDeliveryAddress request)
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

        #endregion

        #region "bankszámlaszám"

        /// <summary>
        /// bankszámlaszám lista
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.RegistrationModule.BankAccounts GetBankAccounts(CompanyGroup.Dto.ServiceRequest.GetBankAccounts request)
        {
            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addBankAccount request cannot be null or empty!");

            CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

            registration.BankAccountList.ForEach(x => x.SplitBankAccount());

            List<CompanyGroup.Dto.RegistrationModule.BankAccount> bankAccountList = registration.BankAccountList.ConvertAll(x => new BankAccountToBankAccount().MapDomainToDto(x));

            CompanyGroup.Dto.RegistrationModule.BankAccounts response = new CompanyGroup.Dto.RegistrationModule.BankAccounts();

            response.Items.AddRange(bankAccountList);

            return response;
        }

        /// <summary>
        /// bankszámlaszám felvitele
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.RegistrationModule.BankAccounts AddBankAccount(CompanyGroup.Dto.ServiceRequest.AddBankAccount request)
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

        /// <summary>
        /// bankszámlaszám adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.BankAccounts UpdateBankAccount(CompanyGroup.Dto.ServiceRequest.UpdateBankAccount request)
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

        /// <summary>
        /// bankszámlaszám törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.BankAccounts RemoveBankAccount(CompanyGroup.Dto.ServiceRequest.RemoveBankAccount request)
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

        #endregion

        #region "kapcsolattartó"

        /// <summary>
        /// kapcsolattartó hozzáadása
        /// </summary>
        /// <param name="request"></param>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons AddContactPerson(CompanyGroup.Dto.ServiceRequest.AddContactPerson request)
        {
            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addBankAccount request cannot be null or empty!");

            request.ContactPerson.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();

            request.ContactPerson.ContactPersonId = request.ContactPerson.ContactPersonId ?? String.Empty;

            CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson = new ContactPersonToContactPerson().MapDtoToDomain(request.ContactPerson);

            registrationRepository.AddContactPerson(request.RegistrationId, contactPerson);

            CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

            List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

            CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons();

            response.Items.AddRange(contactPersonList);

            return response;
        }

        /// <summary>
        /// kapcsolattartó kiolvasása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons GetContactPersons(CompanyGroup.Dto.ServiceRequest.GetContactPerson request)
        {
            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration addBankAccount request cannot be null or empty!");

            CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

            List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

            CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons();

            response.Items.AddRange(contactPersonList);

            return response;
        }

        /// <summary>
        /// kapcsolattartó adatainak módosítása
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons UpdateContactPerson(CompanyGroup.Dto.ServiceRequest.UpdateContactPerson request)
        {
            CompanyGroup.Helpers.DesignByContract.Require((request != null), "Registration updateContactPerson request cannot be null or empty!");

            CompanyGroup.Domain.RegistrationModule.ContactPerson contactPerson = new ContactPersonToContactPerson().MapDtoToDomain(request.ContactPerson);

            registrationRepository.UpdateContactPerson(request.RegistrationId, contactPerson);

            CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

            List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

            CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons();

            response.Items.AddRange(contactPersonList);

            return response;
        }

        /// <summary>
        /// Kapcsolattartó törlése
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPersons RemoveContactPerson(CompanyGroup.Dto.ServiceRequest.RemoveContactPerson request)
        {
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.RegistrationId), "Registration id cannot be null or empty!");
            CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.Id), "ContactPerson id cannot be null or empty!");

            registrationRepository.RemoveContactPerson(request.RegistrationId, request.Id);

            CompanyGroup.Domain.RegistrationModule.Registration registration = registrationRepository.GetByKey(request.RegistrationId);

            List<CompanyGroup.Dto.RegistrationModule.ContactPerson> contactPersonList = registration.ContactPersonList.ConvertAll(x => new ContactPersonToContactPerson().MapDomainToDto(x));

            CompanyGroup.Dto.RegistrationModule.ContactPersons response = new CompanyGroup.Dto.RegistrationModule.ContactPersons();

            response.Items.AddRange(contactPersonList);

            return response;
        }

        #endregion
    }
}
