using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// elfelejtett jelszó a kiküldéshez
    /// </summary>
    public class ForgetPassword : IValidatableObject
    {
        public ForgetPassword(string code, string userName, string password, string email, string companyName, string personName)
        {
            this.Code = code;

            this.UserName = userName;

            this.Password = password;
            
            this.Email = email;

            this.CompanyName = companyName;

            this.PersonName = personName;
        }

        /// <summary>
        /// OK, UserNameNotFound, NotAllowed
        /// </summary>
        public string Code { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { set; get; }

        public string CompanyName { set; get; }

        public string PersonName { set; get; }

        public string GetMessage(string languageId)
        {
            if (this.Code.Equals("OK"))
            {
                return languageId.Equals(CompanyGroup.Domain.Core.Constants.LanguageEnglish) ? "The forget password email message has been sent." : "A jelszóemlékeztető email elküldésre került!";
            }
            if (this.Code.Equals("UserNameNotFound"))
            {
                return languageId.Equals(CompanyGroup.Domain.Core.Constants.LanguageEnglish) ? "The user name not found!" : "A felhasználónév nem található!";
            }
            if (this.Code.Equals("NotAllowed"))
            {
                return languageId.Equals(CompanyGroup.Domain.Core.Constants.LanguageEnglish) ? "The forget password message has not sent!" : "A jelszóemlékeztetőt nem lehet elküldeni!";
            }
            return String.Empty;
        }

        public bool Succeedeed
        {
            get { return this.Code.Equals("OK"); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (String.IsNullOrWhiteSpace(this.UserName))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.verification_UserNameCannotBeNull, new string[] { "UserName" }));
            }

            return validationResults;
        }

    }
}
