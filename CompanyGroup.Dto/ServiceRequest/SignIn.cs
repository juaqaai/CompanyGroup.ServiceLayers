using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// bejelentkezés adatait összefogó osztály
    /// </summary>
    public class SignIn
    {
        public string DataAreaId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string IPAddress { get; set; }

    }
}
