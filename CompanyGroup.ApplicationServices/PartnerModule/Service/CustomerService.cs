using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{

    [ServiceBehavior(UseSynchronizationContext = false,
                     InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     IncludeExceptionDetailInFaults = true),
                     System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    [CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] //create instance and inject dependencies using unity container
    public class CustomerService : ServiceCoreBase, ICustomerService 
    {
        private readonly static double AuthCookieExpiredHours = Helpers.ConfigSettingsParser.GetDouble("AuthCookieExpiredHours", 24d);

        private CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository;

        private CompanyGroup.Domain.PartnerModule.IInvoiceRepository invoiceRepository;

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="customerRepository"></param>
        public CustomerService(CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository, CompanyGroup.Domain.PartnerModule.IInvoiceRepository invoiceRepository, CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (customerRepository == null)
            {
                throw new ArgumentNullException("CustomerRepository");
            }

            this.customerRepository = customerRepository;

            this.invoiceRepository = invoiceRepository;
        }

        /// <summary>
        /// vevő irányítószám lista kiolvasása megadott minta és vállalatkód alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.AddressZipCodes GetAddressZipCodes(CompanyGroup.Dto.ServiceRequest.AddressZipCode request)
        {
            List<CompanyGroup.Domain.PartnerModule.AddressZipCode> addressZipCodes = customerRepository.GetAddressZipCode(request.DataAreaId, request.Prefix);

            return new AddressZipCodeToAddressZipCode().Map(addressZipCodes);
        }

        /// <summary>
        ///  vevő regisztrációs adatok kiolvasása látogatóazonosító és vállalatkód alapján  
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.Registration GetCustomerRegistration(CompanyGroup.Dto.ServiceRequest.GetCustomerRegistration request)
        {
            //ha üres a látogató azonosító
            if (String.IsNullOrEmpty(request.VisitorId))
            {
                return new CompanyGroup.Dto.RegistrationModule.Registration();
            }

            CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

            //ha nincs bejelentkezve
            if (!visitor.IsValidLogin)
            {
                return new CompanyGroup.Dto.RegistrationModule.Registration();
            }

            CompanyGroup.Dto.RegistrationModule.Registration result = new CompanyGroup.Dto.RegistrationModule.Registration();

            List<CompanyGroup.Domain.PartnerModule.BankAccount> bankAccounts = customerRepository.GetBankAccounts(visitor.CompanyId, request.DataAreaId);

            List<CompanyGroup.Domain.PartnerModule.ContactPerson> contactPersons = customerRepository.GetContactPersons(visitor.CompanyId, request.DataAreaId);

            CompanyGroup.Domain.PartnerModule.Customer customer = customerRepository.GetCustomer(visitor.CompanyId, request.DataAreaId);

            List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddresses = customerRepository.GetDeliveryAddress(visitor.CompanyId, request.DataAreaId);

            CompanyGroup.Domain.PartnerModule.MailAddress mailAddress = customerRepository.GetMailAddress(visitor.CompanyId, request.DataAreaId);

            //válasz objektum feltöltés
            result.BankAccounts = bankAccounts.ConvertAll(x => new BankAccountToBankAccount().Map(x));

            List<CompanyGroup.Domain.PartnerModule.ContactPerson> contactPersonList = contactPersons.FindAll(x => x.WebAdmin == false);

            result.ContactPersons = contactPersonList.ConvertAll(x => new ContactPersonToContactPerson().Map(x));

            result.CompanyData = new CustomerToCustomer().Map(customer);

            result.DataRecording = new Dto.RegistrationModule.DataRecording() { Email = "", Name = "", Phone = "" };

            result.DeliveryAddresses = deliveryAddresses.ConvertAll(x => new DeliveryAddressToDeliveryAddress().MapDomainToRegistrationModuleDto(x));

            result.InvoiceAddress = new CustomerToInvoiceAddress().Map(customer);

            result.MailAddress = new MailAddressToMailAddress().Map(mailAddress);

            CompanyGroup.Domain.PartnerModule.ContactPerson webAdministrator = contactPersons.FirstOrDefault(x => x.WebAdmin == true);

            result.WebAdministrator = (webAdministrator == null) ? new ContactPersonToWebAdministrator().Map(new CompanyGroup.Domain.PartnerModule.ContactPerson()) : new ContactPersonToWebAdministrator().Map(webAdministrator);

            return result;
        }

        /// <summary>
        /// szállítási címek lekérdezése (rendelés szállítási címek választás)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.DeliveryAddresses GetDeliveryAddresses(CompanyGroup.Dto.ServiceRequest.GetDeliveryAddresses request)
        {
            CompanyGroup.Dto.PartnerModule.DeliveryAddresses result = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses();

            CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

            if (visitor.IsValidLogin)
            {

                List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddresses = customerRepository.GetDeliveryAddress(visitor.CompanyId, request.DataAreaId);

                result.Items.AddRange(deliveryAddresses.ConvertAll( x => new DeliveryAddressToDeliveryAddress().MapDomainToDto(x) ));
            }

            return result;
        }

        /// <summary>
        /// bejelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor SignIn(CompanyGroup.Dto.ServiceRequest.SignIn request)
        {
            //tárolt eljárás hívással történik a bejelentkezés 
            CompanyGroup.Domain.PartnerModule.LoginInfo loginInfo = customerRepository.SignIn(request.UserName, request.Password, request.DataAreaId);

            //visitor domain objektum létrehozása
            CompanyGroup.Domain.PartnerModule.Visitor visitor = new CompanyGroup.Domain.PartnerModule.Visitor(loginInfo);

            visitor.IPAddress = request.IPAddress;

            visitor.DataAreaId = request.DataAreaId;

            //bejelentkezés keletkezési idejének beállítása
            visitor.CreatedDate = DateTime.Now;

            //bejelentkezés lejárati idejének beállítása
            visitor.ExpiredDate = DateTime.Now.AddHours(CustomerService.AuthCookieExpiredHours);

            //aktív státusz beállítása a bejelentkezést követően
            visitor.Status = LoginStatus.Active;

            //bejelentkezett állapot beállítása
            visitor.SetLoggedIn();

            //ha nem sikerült a bejelentkezés, vagy nem érvényes a bejelentkezés, akkor üres visitor objektum felhasználásával történik a visszatérés
            if (!visitor.LoggedIn) 
            {
                return new VisitorToVisitor().Map(visitor); 
            }

            //ha sikeres a bejelentkezés, akkor le kell kérdezni a vevőhöz tartozó árbesorolás kivételeket és 
            //a profil beállításai közé kell menteni
            visitor.Profile.CustomerPriceGroups = customerRepository.GetCustomerPriceGroups(request.DataAreaId, loginInfo.CompanyId);

            //alapértelmezett képviselő adatainak beállítása 
            visitor.Representative.SetDefault();

            //új profilt tárolni kell
            visitorRepository.Add(visitor);

            //mappelés dto-ra
            return new VisitorToVisitor().Map(visitor);
        }

        /// <summary>
        /// kijelentkezés
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.ServiceResponse.Empty SignOut(CompanyGroup.Dto.ServiceRequest.SignOut request)
        {
            visitorRepository.DisableStatus(request.ObjectId, request.DataAreaId);

            return new CompanyGroup.Dto.ServiceResponse.Empty();
        }

        /// <summary>
        /// vevőhöz tartozó számlák listája
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> GetInvoiceInfo(CompanyGroup.Dto.ServiceRequest.GetInvoiceInfo request)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.VisitorId), "VisitorId cannot be null, or empty!");

            try
            {
                List<CompanyGroup.Dto.PartnerModule.InvoiceInfo> result = new List<CompanyGroup.Dto.PartnerModule.InvoiceInfo>();

                //látogató azonosító alapján olvasása a cahce-ből, vagy a visitor repository-ból 
                CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

//                if (visitor.IsValidLogin)
//                {

                    //visitor.

                    List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo> invoiceInfos = invoiceRepository.GetInvoiceDetailedLineInfo(visitor.CompanyId, CompanyGroup.Domain.Core.Constants.DataAreaIdHrp);

                    List<CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo> filteredInvoiceInfo;

                    //lejárt, kifizetetlen
                    if (CompanyGroup.Domain.PartnerModule.InvoicePaymentType.OverDue.Equals((CompanyGroup.Domain.PartnerModule.InvoicePaymentType) request.PaymentType))
                    {
                        //if (DateTime.Compare(t1, t2) >  0) Console.WriteLine("t1 > t2"); 
                        //if (DateTime.Compare(t1, t2) == 0) Console.WriteLine("t1 == t2");
                        //if (DateTime.Compare(t1, t2) < 0) Console.WriteLine("t1 < t2");

                        filteredInvoiceInfo = invoiceInfos.Where(x => (DateTime.Compare(x.DueDate, DateTime.Today) < 0) && (x.InvoiceCredit > 0)).ToList();
                    }
                    //kifizetetlen
                    else if (CompanyGroup.Domain.PartnerModule.InvoicePaymentType.Unpaid.Equals((CompanyGroup.Domain.PartnerModule.InvoicePaymentType) request.PaymentType))
                    {
                        filteredInvoiceInfo = invoiceInfos.Where(x => (x.InvoiceCredit > 0)).ToList();
                    }
                    //összes
                    else
                    {
                        filteredInvoiceInfo = invoiceInfos;
                    }

                    //számla info aggregátum elkészítése
                    IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.InvoiceDetailedLineInfo>> groupedLineInfos = filteredInvoiceInfo.GroupBy(x => x.InvoiceId).OrderBy(x => x.Key);   //IEnumerable<IGrouping<string, CompanyGroup.Domain.PartnerModule.OrderDetailedLineInfo>>

                    List<CompanyGroup.Domain.PartnerModule.InvoiceInfo> invoiceInfoList = new List<CompanyGroup.Domain.PartnerModule.InvoiceInfo>();

                    foreach (var lineInfo in groupedLineInfos)
                    {
                        CompanyGroup.Domain.PartnerModule.InvoiceInfo invoiceInfo = CompanyGroup.Domain.PartnerModule.InvoiceInfo.Create(lineInfo.ToList());

                        invoiceInfoList.Add(invoiceInfo);
                    }

                    result.AddRange(invoiceInfoList.ConvertAll(x => new InvoiceInfoToInvoiceInfo().Map(x)));
//                }

                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
