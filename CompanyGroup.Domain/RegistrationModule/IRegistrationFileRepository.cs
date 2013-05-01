using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    public interface IRegistrationFileRepository
    {
        /// <summary>
        /// beolvassa a fizikai elérési útról a regisztrációs html template file-t
        /// </summary>
        /// <param name="registrationTemplateFileNameWithPath">regisztrációs template file elérési úttal</param>
        string ReadRegistrationHtmlTemplate(string registrationTemplateFileNameWithPath);

        /// <summary>
        /// regisztrációs file elkészítése 
        /// </summary>
        /// <param name="registrationFileNameWithPath"></param>
        /// <param name="htmlContent"></param>
        void CreateRegistrationFile(string registrationFileNameWithPath, string htmlContent);

        /// <summary>
        /// template-ből generált regisztrációs adatok html string-ben
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <param name="registrationHtml"></param>
        /// <returns></returns>
        string RenderRegistrationDataToHtml(string contractNumber, string registrationHtml);
    }
}
