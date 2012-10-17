using System;
using System.Collections.Generic;

namespace Newsletter.ApplicationService
{
    public abstract class BaseStrategy
    {
        protected static readonly bool testMode = CompanyGroup.Helpers.ConfigSettingsParser.GetInt("TestMode", 1).Equals(1);

        protected static readonly string testModeMailAddress = CompanyGroup.Helpers.ConfigSettingsParser.GetString("TestModeMailAddress", "ajuhasz@hrp.hu");

        protected static readonly string smtpMailServerName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("SmtpMailServerName", "195.30.7.32");

        protected static readonly int SmtpPort = CompanyGroup.Helpers.ConfigSettingsParser.GetInt("SmtpPort", 25);
    }
}
