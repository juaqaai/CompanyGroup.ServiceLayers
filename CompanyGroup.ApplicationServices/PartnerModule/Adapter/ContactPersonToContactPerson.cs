using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class ContactPersonToContactPerson
    {
        /// <summary>
        /// Domain kapcsolattartó -> DTO. kapcsolattartó      
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.ContactPerson Map(CompanyGroup.Domain.PartnerModule.ContactPerson from)
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
                InvoiceInfo = from.InvoiceInfo,
                LastName = from.LastName,
                LeftCompany = from.LeftCompany,
                Newsletter = from.Newsletter,
                Password = from.Password,
                PriceListDownload = from.PriceListDownload,  
                SmsArriveOfGoods = from.SmsArriveOfGoods,
                SmsOfDelivery = from.SmsOfDelivery,
                SmsOrderConfirm = from.SmsOrderConfirm,
                Telephone = from.Telephone,
                UserName = from.UserName,
                WebAdmin = from.WebAdmin, 
                Id = String.Empty
            };
        }

        public CompanyGroup.Dto.PartnerModule.ContactPerson MapToPartnerModuleContactPerson(CompanyGroup.Domain.PartnerModule.ContactPerson from)
        {
            return new CompanyGroup.Dto.PartnerModule.ContactPerson()
            {
                AllowOrder = from.AllowOrder,
                AllowReceiptOfGoods = from.AllowReceiptOfGoods,
                ContactPersonId = from.ContactPersonId,
                Email = from.Email,
                EmailArriveOfGoods = from.EmailArriveOfGoods,
                EmailOfDelivery = from.EmailOfDelivery,
                EmailOfOrderConfirm = from.EmailOfOrderConfirm,
                FirstName = from.FirstName,
                InvoiceInfo = from.InvoiceInfo,
                LastName = from.LastName,
                LeftCompany = from.LeftCompany,
                Newsletter = from.Newsletter,
                Password = from.Password,
                PriceListDownload = from.PriceListDownload,
                SmsArriveOfGoods = from.SmsArriveOfGoods,
                SmsOfDelivery = from.SmsOfDelivery,
                SmsOrderConfirm = from.SmsOrderConfirm,
                Telephone = from.Telephone,
                UserName = from.UserName,
                WebAdmin = from.WebAdmin, 
                Id = 0
            };
        }
    }
}
