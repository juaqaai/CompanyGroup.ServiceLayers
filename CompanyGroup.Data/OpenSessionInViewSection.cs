using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data
{
    /// <summary>
    /// Encapsulates a section of Web/App.config
    /// to declare which session factories are to be created.
    /// Kudos go out to 
    /// http://msdn2.microsoft.com/en-us/library/
    ///    system.configuration.configurationcollectionattribute.aspx
    /// for this technique - it was by far the best overview of the subject.
    /// </summary>

    public class OpenSessionInViewSection : System.Configuration.ConfigurationSection
    {
        [System.Configuration.ConfigurationProperty("sessionFactories", IsDefaultCollection = false)]
        [System.Configuration.ConfigurationCollection(typeof(SessionFactoriesCollection), AddItemName = "sessionFactory", ClearItemsName = "clearFactories")]
        public SessionFactoriesCollection SessionFactories
        {
            get
            {
                SessionFactoriesCollection sessionFactoriesCollection = (SessionFactoriesCollection)base["sessionFactories"];

                return sessionFactoriesCollection;
            }
        }
    }
}
