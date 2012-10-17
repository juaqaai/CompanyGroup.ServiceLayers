using System;
using System.Web.Configuration;
using System.Globalization;

namespace CompanyGroup.Helpers
{
    /// <summary>
    /// konfiguracios beallitasok kiolvasására szolgaló segédosztály
    /// </summary>
    public static class ConfigSettingsParser
    {
        /// <summary>
        /// kiolvassa az adatbazis kapcsolat beallitasait a web.config allomanybol 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ConnectionString(string name)
        {
            try
            {
                return System.Web.Configuration.WebConfigurationManager.ConnectionStrings[ name ].ConnectionString;
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// kiolvassa az appsettings szekciobol a megadott parameterrel rendelkezo értéket
        /// </summary>
        /// <param name="param"></param>
        /// <param name="defaultValue"></param>
        /// <returns>string</returns>
        public static string GetString( string param, string defaultValue )
        {
            string ret = defaultValue;
            try
            {
                ret = System.Web.Configuration.WebConfigurationManager.AppSettings.Get( param );
                return (ret == null) ? defaultValue : ret;
            }
            catch
            {
                return ret;
            }
        }

        /// <summary>
        /// kiolvassa az appsettings szekciobol a megadott parameterrel rendelkezo értéket
        /// </summary>
        /// <param name="sParam"></param>
        /// <returns>String</returns>
        public static string GetString( string param )
        {
            return GetString( param, String.Empty );
        }

        /// <summary>
        /// kiolvassa az appsettings szekciobol a megadott parameterrel rendelkezo értéket
        /// </summary>
        /// <param name="param"></param>
        /// <param name="defaultValue"></param>
        /// <returns>integer</returns>
        public static int GetInt( string param, int defaultValue )
        {
            try
            {
                string configValue = ConfigSettingsParser.GetString( param, null );

                int ret = 0;

                if (configValue == null)
                {
                    return defaultValue;
                }

                if (Int32.TryParse(configValue, out ret))
                {
                    return ret;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

       /// <summary>
        /// kiolvassa az appsettings szekciobol a megadott parameterrel rendelkezo értéket
        /// </summary>
        /// <param name="param"></param>
        /// <returns>integer</returns>
        public static int GetInt( string param )
        {
            return GetInt( param, 0 );
        }

        /// <summary>
        /// kiolvassa az appsettings szekciobol a megadott parameterrel rendelkezo értéket
        /// </summary>
        /// <param name="param"></param>
        /// <param name="defaultValue"></param>
        /// <returns>boolean</returns>
        public static bool GetBoolean( string param, bool defaultValue )
        {
            try
            {
                string configValue = ConfigSettingsParser.GetString(param, null);

                bool ret = false;

                if (configValue == null)
                {
                    return defaultValue;
                }

                if (Boolean.TryParse(configValue, out ret))
                {
                    return ret;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// kiolvassa az appsettings szekciobol a megadott parameterrel rendelkezo értéket
        /// </summary>
        /// <param name="param"></param>
        /// <returns>boolean</returns>
        public static bool GetBoolean( string param )
        {
            return GetBoolean( param, false );
        }

        /// <summary>
        /// kiolvassa az appsettings szekciobol a megadott parameterrel rendelkezo értéket
        /// </summary>
        /// <param name="param"></param>
        /// <param name="defaultValue"></param>
        /// <returns>double</returns>
        public static double GetDouble(string param, double defaultValue)
        {
            try
            {
                string configValue = ConfigSettingsParser.GetString(param, null);

                double ret = 0;

                if (configValue == null)
                {
                    return defaultValue;
                }

                if (double.TryParse(configValue, out ret))
                {
                    return ret;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// kiolvassa az appsettings szekciobol a megadott parameterrel rendelkezo értéket
        /// </summary>
        /// <param name="param"></param>
        /// <returns>double</returns>
        public static double GetDouble(string param)
        {
            return GetInt(param, 0);
        }

    }

}