using System;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// jelszóváltoztatás visszavonás kérés 
    /// </summary>
    public class UndoChangePasswordRequest
    {
        public UndoChangePasswordRequest() : this("") { }

        public UndoChangePasswordRequest(string id)
        { 
            this.Id = id;
        }

        /// <summary>
        /// changepassword log azonosítója
        /// </summary>
        public string Id { get; set; }

    }
}
