namespace CompanyGroup.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Runtime.Serialization.Json;

    public class JsonConverter
    {
        /// <summary>
        /// T -> Json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON<T>(T obj) where T : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);

                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// Json string -> T 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJSON<T>(string json) where T : class
        {
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                return serializer.ReadObject(stream) as T;
            }
        }
    }
}
