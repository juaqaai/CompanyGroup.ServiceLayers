using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    public class ContactPerson : CompanyGroup.Dto.RegistrationModule.ContactPerson
    {
        public ContactPerson(CompanyGroup.Dto.RegistrationModule.ContactPerson contactPerson)
        {
            this.AllowOrder = contactPerson.AllowOrder;
            this.AllowReceiptOfGoods = contactPerson.AllowReceiptOfGoods;
            this.ContactPersonId = contactPerson.ContactPersonId;
            this.Email = contactPerson.Email;
            this.EmailArriveOfGoods = contactPerson.EmailArriveOfGoods;
            this.EmailOfDelivery = contactPerson.EmailOfDelivery;
            this.EmailOfOrderConfirm = contactPerson.EmailOfOrderConfirm;
            this.FirstName = contactPerson.FirstName;
            this.InvoiceInfo = contactPerson.InvoiceInfo;
            this.LastName = contactPerson.LastName;
            this.LeftCompany = contactPerson.LeftCompany;
            this.Newsletter = contactPerson.Newsletter;
            this.Password = contactPerson.Password;
            this.PriceListDownload = contactPerson.PriceListDownload;
            //this.RecId = contactPerson.RecId;
            //this.RefRecId = contactPerson.RefRecId;
            this.SmsArriveOfGoods = contactPerson.SmsArriveOfGoods;
            this.SmsOfDelivery = contactPerson.SmsOfDelivery;
            this.SmsOrderConfirm = contactPerson.SmsOrderConfirm;
            this.Telephone = contactPerson.Telephone;
            this.UserName = contactPerson.UserName;
            this.WebAdmin = contactPerson.WebAdmin;

            this.SelectedItem = false;
        }

        public ContactPerson() { }

        public bool SelectedItem { get; set; }
    }
}
