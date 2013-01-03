using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompanyGroup.Data.Test
{
    [TestClass]
    public class RepositoryBase
    {
        /// <summary>
        /// xml string sorosítás
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
        /// string-ből xml-be visszasorosítás
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
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
