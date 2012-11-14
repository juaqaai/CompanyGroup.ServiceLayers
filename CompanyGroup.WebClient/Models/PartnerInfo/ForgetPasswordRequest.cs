using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// elfelejtett jelszó
    /// </summary>
    public class ForgetPasswordRequest
    {
        public string UserName { get; set; }
    }
}