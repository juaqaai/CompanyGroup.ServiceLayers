using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// jelszómódosítás ellenörzés eredménye
    /// </summary>
    public class ChangePasswordVerify : CompanyGroup.Domain.Core.ValueObject<ChangePasswordVerify>
    {
        /// <summary>
        /// kód alapján beállítja a message jellemző értékét
        /// </summary>
        /// <param name="code"></param>
        public ChangePasswordVerify(string code)
        {
            this.Code = code;

            if ("UserNamePasswordOK".Equals(code))
            {
                this.Message = Resources.Messages.verification_UserNamePasswordOK;
            }
            else if ("PasswordAleradyExists".Equals(code))
            {
                this.Message = Resources.Messages.verification_PasswordAleradyExists;
            }
            else if ("UserNamePasswordNotFound".Equals(code))
            {
                this.Message = Resources.Messages.verification_UserNamePasswordNotFound;
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
            get { return "UserNamePasswordOK".Equals(this.Code); }
        }
    }
}
