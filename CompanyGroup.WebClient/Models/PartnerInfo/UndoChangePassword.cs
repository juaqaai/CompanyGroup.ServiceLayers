using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// jelszó visszavonás eredmény
    /// </summary>
    public class UndoChangePassword : CompanyGroup.Dto.PartnerModule.UndoChangePassword
    {
        public UndoChangePassword(CompanyGroup.Dto.PartnerModule.UndoChangePassword undoChangePassword, CompanyGroup.WebClient.Models.Visitor visitor)
        {
            this.Message = undoChangePassword.Message;

            this.Succeeded = undoChangePassword.Succeeded;

            this.Visitor = visitor;
        }

        public CompanyGroup.WebClient.Models.Visitor Visitor { get; set; }
    }
}
