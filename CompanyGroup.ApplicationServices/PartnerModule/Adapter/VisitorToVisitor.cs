using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.ApplicationServices.PartnerModule
{
    public class VisitorToVisitor
    {
        /// <summary>
        /// Domain Visitor -> DTO visitor
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public CompanyGroup.Dto.PartnerModule.Visitor Map(CompanyGroup.Domain.PartnerModule.Visitor from)
        {
            return new CompanyGroup.Dto.PartnerModule.Visitor() { CompanyId = from.CompanyId, 
                                                                  CompanyName = from.CompanyName, 
                                                                  History = CreateHistory(from.Profile.Histories),
                                                                  Id = from.Id.ToString(), 
                                                                  IsValidLogin = from.IsValidLogin,
                                                                  LoggedIn = from.LoggedIn, 
                                                                  Permission = CreatePermission(from.Permission),  
                                                                  PersonId = from.PersonId, 
                                                                  PersonName = from.PersonName,
                                                                  Roles = from.Roles,
                                                                  Currency = from.Currency,
                                                                  InventLocation = from.InventLocation,
                                                                  LanguageId = from.LanguageId,
                                                                  PaymTermId = from.PaymTermId,
                                                                  BscAuthorized = from.PartnerModel.Equals(PartnerModel.Both) || from.PartnerModel.Equals(PartnerModel.Bsc),
                                                                  HrpAuthorized = from.PartnerModel.Equals(PartnerModel.Both) || from.PartnerModel.Equals(PartnerModel.Hrp),
                                                                  Representative = CreateRepresentative(from.Representative)
            };
        }

        private CompanyGroup.Dto.PartnerModule.Permission CreatePermission(CompanyGroup.Domain.PartnerModule.Permission permission)
        {
            return new Dto.PartnerModule.Permission()
            {
                CanOrder = permission.CanOrder,
                InvoiceInfoEnabled = permission.InvoiceInfoEnabled,
                IsWebAdministrator = permission.IsWebAdministrator,
                PriceListDownloadEnabled = permission.PriceListDownloadEnabled,
                RecieveGoods = permission.RecieveGoods
            };
        }

        private List<string> CreateHistory(HashSet<Domain.PartnerModule.History> histories)
        { 
            List<string> to = new List<string>();

            histories.ToList().ForEach(h => to.Add(h.Url));

            return to;
        }

        private CompanyGroup.Dto.PartnerModule.Representative CreateRepresentative(CompanyGroup.Domain.PartnerModule.Representative from)
        {
            return new CompanyGroup.Dto.PartnerModule.Representative() 
                   { 
                       Email = from.Email, 
                       Id = from.Id, 
                       Mobile = from.Mobile, 
                       Name = from.Name,
                       Phone = CreatePhoneNumber(from.Phone, from.Extension)
                   };
        }

        private static string CreatePhoneNumber(string phone, string extension)
        { 
            if (String.IsNullOrEmpty(phone) && String.IsNullOrEmpty(extension))
            {
                return CompanyGroup.Domain.Core.Constants.CompanyBasePhoneNumber;
            }

            if ((String.IsNullOrEmpty(phone) || phone.Equals(CompanyGroup.Domain.Core.Constants.CompanyBasePhoneNumber)) && !String.IsNullOrEmpty(extension))
            {
                return CompanyGroup.Domain.Core.Constants.CompanyBasePhoneNumber.Replace("600", extension);
            }

            return String.IsNullOrEmpty(phone) ? CompanyGroup.Domain.Core.Constants.CompanyBasePhoneNumber : phone;
        }

        //private string ConvertFromHistoryToString(Domain.PartnerModule.History from)
        //{ 
        //    from.RequestParameters.
        //    from.Url
        //}
    }
}
