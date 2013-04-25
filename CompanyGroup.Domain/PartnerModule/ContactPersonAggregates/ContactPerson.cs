using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// kapcsolattartó entitás
    /// ContactPersonId	LastName	FirstName	Email	CellularPhone	Phone	PhoneLocal	AllowOrder	AllowReceiptOfGoods	
    /// SmsArriveOfGoods	SmsOrderConfirm	SmsOfDelivery	EmailArriveOfGoods	EmailOfOrderConfirm	EmailOfDelivery	
    /// WebAdmin	PriceListDownload	InvoiceInfo	UserName	Password	LeftCompany	Newsletter	RecId	RefRecId
    /// </summary>
    public class ContactPerson : CompanyGroup.Domain.Core.EntityBase, IValidatableObject
    {
        public ContactPerson(string contactPersonId, string lastName, string firstName, string email, string cellularPhone, string phone, string phoneLocal,
                             bool allowOrder, bool allowReceiptOfGoods,
                             bool smsArriveOfGoods, bool smsOrderConfirm, bool smsOfDelivery,
                             bool emailArriveOfGoods, bool emailOfOrderConfirm, bool emailOfDelivery,
                             bool webAdmin, bool priceListDownload, bool invoiceInfo,
                             string userName, string password, bool leftCompany, bool newsletter, long recId, long refRecId)
            : this(contactPersonId, lastName, firstName, email, cellularPhone, phone, phoneLocal,
                             allowOrder, allowReceiptOfGoods,
                             smsArriveOfGoods, smsOrderConfirm, smsOfDelivery,
                             emailArriveOfGoods, emailOfOrderConfirm, emailOfDelivery,
                             webAdmin, priceListDownload, invoiceInfo,
                             userName, password, leftCompany, newsletter, recId, refRecId, new List<string>()) { }

        public ContactPerson(string contactPersonId, string lastName, string firstName, string email, string cellularPhone, string phone, string phoneLocal,
                             bool allowOrder, bool allowReceiptOfGoods, 
                             bool smsArriveOfGoods, bool smsOrderConfirm, bool smsOfDelivery, 
                             bool emailArriveOfGoods, bool emailOfOrderConfirm, bool emailOfDelivery, 
                             bool webAdmin, bool priceListDownload, bool invoiceInfo,
                             string userName, string password, bool leftCompany, bool newsletter, long recId, long refRecId, List<string> positions)
        {
            this.ContactPersonId = contactPersonId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.AllowOrder = allowOrder;
            this.AllowReceiptOfGoods = allowReceiptOfGoods;
            this.SmsArriveOfGoods = smsArriveOfGoods;
            this.SmsOrderConfirm = smsOrderConfirm;
            this.SmsOfDelivery = smsOfDelivery;
            this.EmailArriveOfGoods = emailArriveOfGoods;
            this.EmailOfOrderConfirm = emailOfOrderConfirm;
            this.EmailOfDelivery = emailOfDelivery;
            this.WebAdmin = webAdmin;
            this.PriceListDownload = priceListDownload;
            this.InvoiceInfo = invoiceInfo;
            this.UserName = userName;
            this.Password = password;
            this.Newsletter = newsletter;
            this.Telephone = !String.IsNullOrWhiteSpace(cellularPhone) ? cellularPhone : !String.IsNullOrWhiteSpace(phone) ? phone : "";
            this.Email = email;
            this.LeftCompany = leftCompany;
            this.RecId = recId;
            this.RefRecId = refRecId;
            this.Positions = positions;
        }

        public ContactPerson() : this("", "", "", "", "", "", "", false, false, false, false, false, false, false, false, false, false, false, "", "", false, false, 0, 0, new List<string>()) { }

        /// <summary>
        /// kapcsolattartó egyedi azonosító
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("ContactPersonId")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string ContactPersonId { get; set; }

        /// <summary>
        /// kapcsolattartó vezetékneve
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("FirstName")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string FirstName { get; set; }

        /// <summary>
        ///kapcsolattartó keresztneve
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("LastName")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string LastName { get; set; }

        /// <summary>
        /// engedélyezett-e a rendelés?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("AllowOrder")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool AllowOrder { get; set; }

        /// <summary>
        /// engedélyezett-e az áru átvétele?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("AllowReceiptOfGoods")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool AllowReceiptOfGoods { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva az árubeérkezésről?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("SmsArriveOfGoods")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool SmsArriveOfGoods { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva a megrendelésről?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("SmsOrderConfirm")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool SmsOrderConfirm { get; set; }

        /// <summary>
        /// sms küldés be van-e állítva a kiszállításról?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("SmsOfDelivery")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool SmsOfDelivery { get; set; }

        /// <summary>
        /// email küldés be van-e állítva az árubeérkezésről?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("EmailArriveOfGoods")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool EmailArriveOfGoods { get; set; }

        /// <summary>
        /// email küldés be van-e állítva a megrendelésről?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("EmailOfOrderConfirm")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool EmailOfOrderConfirm { get; set; }

        /// <summary>
        /// email küldés be van-e állítva a kiszállításról?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("EmailOfDelivery")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool EmailOfDelivery { get; set; }

        /// <summary>
        /// Webadmin-e a kapcsolattartó?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("WebAdmin")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool WebAdmin { get; set; }

        /// <summary>
        /// árlistát letölthet-e?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("PriceListDownload")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool PriceListDownload { get; set; }

        /// <summary>
        /// számlainformációt nézhet-e?
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("InvoiceInfo")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool InvoiceInfo { get; set; }

        /// <summary>
        /// webes belépési név
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("UserName")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string UserName { get; set; }

        /// <summary>
        /// webes belépési jelszó
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Password")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Password { get; set; }

        /// <summary>
        /// hírlevél beállítás érvényben, van-e? 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Newsletter")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool Newsletter { get; set; }

        /// <summary>
        /// vevő teljes neve 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", this.LastName, this.FirstName);
            }
            set { }
        }

        /// <summary>
        /// mobil, vagy vonalas telefonszám 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Telephone")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Telephone { get; set; }

        /// <summary>
        /// kapcsolattartó email cím
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Email")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Email { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("LeftCompany")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public bool LeftCompany { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("RecId")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public long RecId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("RefRecId")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public long RefRecId { get; set; }

        /// <summary>
        /// megadott pozíciók
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Positions")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public List<string> Positions { get; set; }

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();


            if (String.IsNullOrEmpty(ContactPersonId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "ContactPersonId" }));
            }

            return validationResults;
        }

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return String.IsNullOrWhiteSpace(this.ContactPersonId);
        }


        /// <summary>
        /// override-olt egyenlőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ContactPerson))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            ContactPerson item = (ContactPerson)obj;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.ContactPersonId == this.ContactPersonId;
            }
        }

        /// <summary>
        /// hash code segédmetódus
        /// </summary>
        /// <returns></returns>
        public override int GetRequestedHashCode()
        {
            return this.ContactPersonId.GetHashCode() ^ 31;
        }
    }
}
