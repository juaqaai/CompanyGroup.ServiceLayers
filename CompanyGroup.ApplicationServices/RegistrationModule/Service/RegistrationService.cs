using System;
using System.Collections.Generic;
using System.ServiceModel;

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

        public RegistrationService(CompanyGroup.Domain.RegistrationModule.IRegistrationRepository registrationRepository, 
                                   CompanyGroup.Domain.PartnerModule.IVisitorRepository visitorRepository) : base(visitorRepository)
        {
            if (registrationRepository == null)
            {
                throw new ArgumentNullException("RegistrationRepository");
            }

            this.registrationRepository = registrationRepository;
        }

        /// <summary>
        /// kulcs alapján a megkezdett regisztrációs adatok kiolvasása
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

                //bejelentkezett felhasználó lekérdezése
                CompanyGroup.Domain.PartnerModule.Visitor visitor = base.GetVisitor(request.VisitorId);

                
                if (!String.IsNullOrEmpty(request.RegistrationId))
                {
                    registration = registrationRepository.GetByKey(request.RegistrationId);
                }

                //ha nincs megkezdett regisztráció 
                if (registration == null || MongoDB.Bson.ObjectId.Empty.Equals(registration.Id))
                {
                    //üres regisztráció létrehozása
                    CompanyGroup.Domain.RegistrationModule.Registration newRegistration = CompanyGroup.Domain.RegistrationModule.Factory.CreateRegistration();

                    //új regisztrációs azonosító létrehozása
                    newRegistration.Id = MongoDB.Bson.ObjectId.GenerateNewId();

                    newRegistration.CompanyId = visitor.IsValidLogin ? visitor.CustomerId : String.Empty;

                    newRegistration.PersonId = visitor.IsValidLogin ? visitor.PersonId : String.Empty;

                    newRegistration.VisitorId = visitor.IsValidLogin ? request.VisitorId : String.Empty;

                    //új regisztráció mentés 
                    registrationRepository.Add(newRegistration);

                    //új regisztráció visszaolvasás
                    registration = registrationRepository.GetByKey(newRegistration.Id.ToString());
                }

                //ha még mindíg nincs meg a regisztráció, akkor egy új létrehozása szükséges
                if (registration == null)
                {
                    registration = CompanyGroup.Domain.RegistrationModule.Factory.CreateRegistration();
                }

                //bankszámlaszámok szétdarabolása
                registration.BankAccountList.ForEach(x => x.SplitBankAccount());

                CompanyGroup.Dto.RegistrationModule.Registration response = new RegistrationToRegistration().MapDomainToDto(registration);

                response.Visitor = new CompanyGroup.ApplicationServices.PartnerModule.VisitorToVisitor().Map(visitor);

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
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrWhiteSpace(request.RegistrationId), "Registration id cannot be null or empty!");

                registrationRepository.Post(request.RegistrationId);

                return new CompanyGroup.Dto.ServiceResponse.PostRegistration() { Message = "", Successed = true };
            }
            catch(Exception ex)
            {
                return new CompanyGroup.Dto.ServiceResponse.PostRegistration() { Message = ex.Message, Successed = false };
            }
        }

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
    }
}
