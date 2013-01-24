using System;

namespace CompanyGroup.Dto.PartnerModule
{
    public class ChangePasswordRequest
    {
        public ChangePasswordRequest() : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty) { }

        public ChangePasswordRequest(string language, string userName, string oldPassword, string newPassword, string visitorId)
        {
            this.Language = language;
            this.UserName = userName;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
            this.VisitorId = visitorId;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// régi jelszó
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// új jelszó
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        public string VisitorId { get; set; }
    }
}
