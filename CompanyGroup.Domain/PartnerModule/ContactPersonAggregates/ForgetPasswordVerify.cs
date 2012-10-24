using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// elfelejtett jelszó ellenörzés eredménye
    /// </summary>
    public class ForgetPasswordVerify : CompanyGroup.Domain.Core.ValueObject<ForgetPasswordVerify>
    {
        /// <summary>
        /// kód alapján beállítja a message jellemző értékét
        /// </summary>
        /// <param name="code"></param>
        public ForgetPasswordVerify(string code)
        {
            this.Code = code;

            if ("UserNameOK".Equals(code))
            {
                this.Message = Resources.Messages.verification_UserNameOK;
            }
            else if ("UserNameNotFound".Equals(code))
            {
                this.Message = Resources.Messages.verification_UserNameNotFound;
            }
            else if ("NotAllowed".Equals(code))
            {
                this.Message = Resources.Messages.verification_ForgetPasswordNotAllowed;
            }
            else
            {
                this.Message = String.Empty;
            }
        }

        /// <summary>
        /// hibakód
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// válaszüzenet
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// sikeres
        /// </summary>
        public bool Success
        {
            get { return "UserNameOK".Equals(this.Code); }
        }
    }
}
