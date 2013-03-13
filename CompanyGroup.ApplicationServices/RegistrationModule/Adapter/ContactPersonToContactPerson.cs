using System;
using System.Collections.Generic;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class ContactPersonToContactPerson
    {
        /// <summary>
        /// Domain kapcsolattartó -> DTO. kapcsolattartó      
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPerson MapDomainToDto(CompanyGroup.Domain.RegistrationModule.ContactPerson from)
        {
            return new CompanyGroup.Dto.RegistrationModule.ContactPerson() 
            { 
                AllowOrder = from.AllowOrder, 
                AllowReceiptOfGoods = from.AllowReceiptOfGoods,
                ContactPersonId = from.ContactPersonId,
                Email = from.Email,
                EmailArriveOfGoods = from.EmailArriveOfGoods,
                EmailOfDelivery = from.EmailOfDelivery,
                EmailOfOrderConfirm = from.EmailOfOrderConfirm,
                FirstName = from.FirstName,
                Id = from.Id.ToString(),
                InvoiceInfo = from.InvoiceInfo,
                LastName = from.LastName,
                LeftCompany = from.LeftCompany,
                Newsletter = from.Newsletter,
                Password = from.Password,
                PriceListDownload = from.PriceListDownload,  
                RecId = from.RecId,
                SmsArriveOfGoods = from.SmsArriveOfGoods,
                SmsOfDelivery = from.SmsOfDelivery,
                SmsOrderConfirm = from.SmsOrderConfirm,
                Telephone = from.Telephone,
                UserName = from.UserName,
                WebAdmin = from.WebAdmin
            };
        }

        /// <summary>
        /// Dto kapcsolattartó -> Domain kapcsolattartó  
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.RegistrationModule.ContactPerson MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.ContactPerson from)
        {
            return new CompanyGroup.Domain.RegistrationModule.ContactPerson()
            {
                AllowOrder = from.AllowOrder,
                AllowReceiptOfGoods = from.AllowReceiptOfGoods,
                ContactPersonId = from.ContactPersonId,
                Email = from.Email,
                EmailArriveOfGoods = from.EmailArriveOfGoods,
                EmailOfDelivery = from.EmailOfDelivery,
                EmailOfOrderConfirm = from.EmailOfOrderConfirm,
                FirstName = from.FirstName,
                Id = MongoDB.Bson.ObjectId.Parse( from.Id ),
                InvoiceInfo = from.InvoiceInfo,
                LastName = from.LastName,
                LeftCompany = from.LeftCompany,
                Newsletter = from.Newsletter,
                Password = from.Password,
                PriceListDownload = from.PriceListDownload,
                RecId = from.RecId,
                FullName = String.Format("{0} {1}", from.FirstName, from.LastName),
                SmsArriveOfGoods = from.SmsArriveOfGoods,
                SmsOfDelivery = from.SmsOfDelivery,
                SmsOrderConfirm = from.SmsOrderConfirm,
                Telephone = from.Telephone,
                UserName = from.UserName,
                WebAdmin = from.WebAdmin
            };
        }
    }
}
