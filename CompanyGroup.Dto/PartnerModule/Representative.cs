using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.PartnerModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Representative", Namespace = "CompanyGroup.Dto.PartnerModule")]
    public class Representative
    {
        public Representative()
        {
            this.Id = String.Empty;
            this.Name = String.Empty;
            this.Email = String.Empty;
            this.Phone = String.Empty;
            this.Mobile = String.Empty;
        }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Name", Order = 2)]
        public string Name { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Email", Order = 3)]
        public string Email { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Phone", Order = 4)]
        public string Phone { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Mobile", Order = 5)]
        public string Mobile { get; set; }
    }
}
