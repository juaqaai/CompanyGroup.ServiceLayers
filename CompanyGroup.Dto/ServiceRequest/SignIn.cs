using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// bejelentkezés adatait összefogó osztály
    /// </summary>
    public class SignIn
    {
        public SignIn() : this("", "", "", "") { }

        public SignIn(string dataAreaId, string userName, string password, string ipAddress)
        {
            this.DataAreaId = dataAreaId;

            this.UserName = userName;

            this.Password = password;

            this.IPAddress = ipAddress;
        }

        public string DataAreaId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string IPAddress { get; set; }

    }
}
