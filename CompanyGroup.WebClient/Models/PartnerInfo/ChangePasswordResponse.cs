using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// jelszócsere view model
    /// </summary>
    public class ChangePasswordResponse
    {
        public ChangePasswordResponse(CompanyGroup.Dto.PartnerModule.ChangePassword changePassword)
        {
            this.Message = changePassword.Message;

            this.OperationSucceeded = changePassword.OperationSucceeded;

            this.SendMailSucceeded = changePassword.SendMailSucceeded;

            this.Visitor = new Visitor(changePassword.Visitor); 
        }

        /// <summary>
        /// jelszócsere művelet sikeresen megtörtént
        /// </summary>
        public bool OperationSucceeded { set; get; }

        /// <summary>
        /// jelszócsere email küldés sikeresen megtörtént
        /// </summary>
        public bool SendMailSucceeded { set; get; }

        /// <summary>
        /// üzenet szövege
        /// </summary>
        public string Message { set; get; }

        /// <summary>
        /// látogató adatok
        /// </summary>
        public Visitor Visitor { get; set; }
    }
}