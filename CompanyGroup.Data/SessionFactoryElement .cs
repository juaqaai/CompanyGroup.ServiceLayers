using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Configuration_StringValidator = System.Configuration.StringValidator;

namespace CompanyGroup.Data
{
    public class SessionFactoryElement : System.Configuration.ConfigurationElement
    {
        public SessionFactoryElement() { }

        public SessionFactoryElement(string name, string configPath)
        {
            Name = name;
            FactoryConfigPath = configPath;
        }

        [System.Configuration.ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "Not Supplied")]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [System.Configuration.ConfigurationProperty("factoryConfigPath", IsRequired = true, DefaultValue = "Not Supplied")]
        public string FactoryConfigPath
        {
            get { return (string)this["factoryConfigPath"]; }
            set { this["factoryConfigPath"] = value; }
        }

        [System.Configuration.ConfigurationProperty("isTransactional", IsRequired = false, DefaultValue = false)]
        public bool IsTransactional
        {
            get { return (bool)this["isTransactional"]; }
            set { this["isTransactional"] = value; }
        }
    }
}
