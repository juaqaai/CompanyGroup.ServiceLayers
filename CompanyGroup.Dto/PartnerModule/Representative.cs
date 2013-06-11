using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Dto.PartnerModule
{
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

        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }
    }

    /// <summary>
    /// képviselők DTO
    /// </summary>
    public class Representatives
    {
        public Representatives()
        {
            this.Items = new List<Representative>();
        }

        public List<Representative> Items { get; set; }


    }
}
