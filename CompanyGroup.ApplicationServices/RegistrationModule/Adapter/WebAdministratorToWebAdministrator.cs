using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyGroup.ApplicationServices.RegistrationModule
{
    public class WebAdministratorToWebAdministrator
    {
        /// <summary>
        /// Domain webadmin -> DTO. webadmin      
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.RegistrationModule.WebAdministrator MapDomainToDto(CompanyGroup.Domain.RegistrationModule.WebAdministrator from)
        {
            return new CompanyGroup.Dto.RegistrationModule.WebAdministrator() 
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
                RecId = from.RecId, 
                RefRecId = from.RefRecId, 
                Positions = from.Positions
            };
        }

        public CompanyGroup.Domain.RegistrationModule.WebAdministrator MapDtoToDomain(CompanyGroup.Dto.RegistrationModule.WebAdministrator from)
        {
            return new CompanyGroup.Domain.RegistrationModule.WebAdministrator()
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
                WebAdmin = true, 
                RecId = from.RecId, 
                RefRecId = from.RefRecId, 
                Positions = from.Positions
            };
        }
    }
}
