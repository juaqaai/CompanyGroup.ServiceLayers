using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// jelszóváltoztatás visszavonás kérés 
    /// </summary>
    [System.Runtime.Serialization.DataContract(Name = "UndoChangePassword", Namespace = "CompanyGroup.Dto.ServiceRequest")]
    public class UndoChangePassword
    {
        public UndoChangePassword() : this("") { }

        public UndoChangePassword(string id)
        { 
            this.Id = id;
        }

        /// <summary>
        /// changepassword log azonosítója
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

    }
}
