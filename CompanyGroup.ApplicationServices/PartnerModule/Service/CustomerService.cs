using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.PartnerModule
{

    //[ServiceBehavior(UseSynchronizationContext = false,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 IncludeExceptionDetailInFaults = true),
    //                 System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Allowed)]
    //[CompanyGroup.ApplicationServices.InstanceProviders.UnityInstanceProviderServiceBehavior()] //create instance and inject dependencies using unity container
    public class CustomerService : ServiceCoreBase, ICustomerService 
    {
        private CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository;

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="customerRepository"></param>
        public CustomerService(CompanyGroup.Domain.PartnerModule.ICustomerRepository customerRepository, CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (customerRepository == null)
            {
                throw new ArgumentNullException("CustomerRepository");
            }

            this.customerRepository = customerRepository;
        }

        /// <summary>
        /// vevő irányítószám lista kiolvasása megadott minta és vállalatkód alapján
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.AddressZipCodes GetAddressZipCodes(CompanyGroup.Dto.PartnerModule.AddressZipCodeRequest request)
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
        public CompanyGroup.Dto.RegistrationModule.Registration GetCustomerRegistration(CompanyGroup.Dto.PartnerModule.GetCustomerRegistrationRequest request)
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

            List<CompanyGroup.Domain.PartnerModule.BankAccount> bankAccounts = customerRepository.GetBankAccounts(visitor.CustomerId, request.DataAreaId);

            List<CompanyGroup.Domain.PartnerModule.ContactPerson> contactPersons = customerRepository.GetContactPersons(visitor.CustomerId, request.DataAreaId);

            CompanyGroup.Domain.PartnerModule.Customer customer = customerRepository.GetCustomer(visitor.CustomerId, request.DataAreaId);

            List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddresses = customerRepository.GetDeliveryAddress(visitor.CustomerId, request.DataAreaId);

            CompanyGroup.Domain.PartnerModule.MailAddress mailAddress = customerRepository.GetMailAddress(visitor.CustomerId, request.DataAreaId);

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

            result.Visitor = new VisitorToVisitor().Map(visitor);

            return result;
        }

        /// <summary>
        /// szállítási címek lekérdezése (rendelés szállítási címek választás)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.DeliveryAddresses GetDeliveryAddresses(CompanyGroup.Dto.PartnerModule.GetDeliveryAddressesRequest request)
        {
            CompanyGroup.Dto.PartnerModule.DeliveryAddresses result = new CompanyGroup.Dto.PartnerModule.DeliveryAddresses();

            CompanyGroup.Domain.PartnerModule.Visitor visitor = this.GetVisitor(request.VisitorId);

            if (visitor.IsValidLogin)
            {

                List<CompanyGroup.Domain.PartnerModule.DeliveryAddress> deliveryAddresses = customerRepository.GetDeliveryAddress(visitor.CustomerId, request.DataAreaId);

                result.Items.AddRange(deliveryAddresses.ConvertAll( x => new DeliveryAddressToDeliveryAddress().MapDomainToDto(x) ));
            }

            return result;
        }

    }
}
