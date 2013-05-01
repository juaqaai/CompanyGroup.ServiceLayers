using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// jelszócsere adatok
    /// </summary>
    public class ChangePassword
    {
        /// <summary>
        /// jelszócsere
        /// </summary>
        public ChangePassword() : this(false, false, String.Empty, new CompanyGroup.Dto.PartnerModule.Visitor())
        { 
        }

        /// <summary>
        /// jelszócsere
        /// </summary>
        /// <param name="operationSucceeded"></param>
        /// <param name="sendMailSucceeded"></param>
        /// <param name="message"></param>
        /// <param name="visitor"></param>
        public ChangePassword(bool operationSucceeded, bool sendMailSucceeded, string message, Visitor visitor)
        {
            this.OperationSucceeded = operationSucceeded;

            this.SendMailSucceeded = sendMailSucceeded;

            this.Message = message;

            this.Visitor = visitor;
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
        public Visitor Visitor { set; get; }
    }
}
