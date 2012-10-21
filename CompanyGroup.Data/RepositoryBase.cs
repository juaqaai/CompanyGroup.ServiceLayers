using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data
{
    public abstract class RepositoryBase
    {
        #region "business connector beállítások"

        protected static readonly string UserName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("UserName", "juhasza");

        protected static readonly string Password = CompanyGroup.Helpers.ConfigSettingsParser.GetString("Password", "juha");

        protected static readonly string Domain = CompanyGroup.Helpers.ConfigSettingsParser.GetString("Domain", "hrp.hu");

        protected static readonly string Language = CompanyGroup.Helpers.ConfigSettingsParser.GetString("Language", "en-gb");

        protected static readonly string ObjectServer = CompanyGroup.Helpers.ConfigSettingsParser.GetString("ObjectServer", "AXOS3@AXOS3:2799");

        #endregion

        /// <summary>
        /// objektum sorosítása
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string Serialize<T>(T obj)
        {
            System.Xml.XmlWriter writer = null;

            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                writer = System.Xml.XmlWriter.Create(sb);

                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);

                string tmp = sb.ToString();

                return tmp;
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

        }

        /// <summary>
        /// visszaállítás objektumba
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <example>
        /// CarCollection cars = null;
        ///string path = "cars.xml";
        ///XmlSerializer serializer = new XmlSerializer(typeof(CarCollection));
        ///StreamReader reader = new StreamReader(path);
        ///cars = (CarCollection)serializer.Deserialize(reader);
        ///reader.Close();
        /// </example>
        /// <returns></returns>
        protected T DeSerialize<T>(string xml)
        {
            System.Xml.XmlReader xmlReader = null;
            try
            {
                System.IO.StringReader stringReader = new System.IO.StringReader(xml);

                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

                xmlReader = new System.Xml.XmlTextReader(stringReader);

                T obj = (T)serializer.Deserialize(xmlReader);

                return obj;
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }
        }


    }
}
