using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.MaintainModule
{
    
    public class ProductManager
    {
        public ProductManager(string employeeId, string name, string email, string extension, string mobile)
        {
            this.EmployeeId = employeeId;

            this.Name = name;

            this.Email = email;

            this.Extension = extension;

            this.Mobile = mobile;
        }

        public string EmployeeId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Extension { get; set; }

        public string Mobile { get; set; }
    }
}
