using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    [System.Runtime.Serialization.DataContract(Name = "UndoChangePassword", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class UndoChangePassword
    {
        public UndoChangePassword() : this("", "") { }

        public UndoChangePassword(string id, string visitorId)
        { 
            this.Id = id;
            this.VisitorId = visitorId;
        }

        /// <summary>
        /// changepassword log azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        /// <summary>
        /// látogató azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "VisitorId", Order = 2)]
        public string VisitorId { get; set; }
    }
}
