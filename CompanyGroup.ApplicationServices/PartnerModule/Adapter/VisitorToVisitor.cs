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
            return new CompanyGroup.Dto.PartnerModule.Visitor() { CompanyId = from.CustomerId, 
                                                                  CompanyName = from.CustomerName, 
                                                                  //History = CreateHistory(from.Profile.Histories),
                                                                  Id = from.VisitorId, 
                                                                  IsValidLogin = from.IsValidLogin,
                                                                  LoggedIn = from.LoggedIn,
                                                                  Permission = ConvertPermission(from.Permission),  
                                                                  PersonId = from.PersonId, 
                                                                  PersonName = from.PersonName,
                                                                  Roles = from.Roles,
                                                                  Currency = from.Currency,
                                                                  InventLocationBsc = from.InventLocationIdBsc,
                                                                  InventLocationHrp = from.InventLocationIdHrp, 
                                                                  LanguageId = from.LanguageId,
                                                                  PaymTermId = from.PaymTermId,
                                                                  BscAuthorized = from.IsAuthorizedInBsc(),
                                                                  HrpAuthorized = from.IsAuthorizedInHrp(),
                                                                  Representatives = ConvertRepresentatives(from.Representatives)
            };
        }

        private CompanyGroup.Dto.PartnerModule.Permission ConvertPermission(CompanyGroup.Domain.PartnerModule.Permission permission)
        {
            return new CompanyGroup.Dto.PartnerModule.Permission()
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

        private CompanyGroup.Dto.PartnerModule.Representatives ConvertRepresentatives(CompanyGroup.Domain.PartnerModule.Representatives representative)
        {
            CompanyGroup.Dto.PartnerModule.Representatives response = new Dto.PartnerModule.Representatives();

            response.Items = representative.ConvertAll(x => ConvertRepresentative(x));

             return response;
        }

        private CompanyGroup.Dto.PartnerModule.Representative ConvertRepresentative(CompanyGroup.Domain.PartnerModule.Representative representative)
        {
            return new CompanyGroup.Dto.PartnerModule.Representative() 
                   {
                       Email = representative.Email,
                       Id = representative.Id,
                       Mobile = representative.Mobile,
                       Name = representative.Name,
                       Phone = representative.Phone
                   };
        }



    }
}
