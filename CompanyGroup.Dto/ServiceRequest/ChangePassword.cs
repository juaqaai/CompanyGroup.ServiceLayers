using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "ChangePassword", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class ChangePassword
    {
        public ChangePassword() : this("", "", "", "", "") { }

        public ChangePassword(string language, string userName, string oldPassword, string newPassword, string visitorId)
        { 
            Language = language;
            UserName = userName;
            OldPassword = oldPassword;
            NewPassword = newPassword;
            VisitorId = visitorId;
        }

        /// <summary>
        /// választott nyelv
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Language", Order = 1)]
        public string Language { get; set; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "UserName", Order = 2)]
        public string UserName { get; set; }

        /// <summary>
        /// régi jelszó
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "OldPassword", Order = 3)]
        public string OldPassword { get; set; }

        /// <summary>
        /// régi jelszó
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "NewPassword", Order = 4)]
        public string NewPassword { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 5)]
        public string VisitorId { get; set; }
    }
}
