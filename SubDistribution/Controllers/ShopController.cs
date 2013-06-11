using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SubDistribution.Controllers
{
    public class ShopController : BaseController
    {
        /// <summary>
        /// GET api/Shop/BravoPhone
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage BravoPhone()
        {
            try
            {
                HttpResponseMessage response = this.GetXmlData("http://www.bravophone.hu", "xmlnew", "dd2c617aae97897d3cdc0a3609b0c910");

                string result = response.Content.ReadAsStringAsync().Result;

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                xmlDoc.LoadXml(result);


                //SubDistribution.Models.Product result = response.Content.ReadAsAsync<SubDistribution.Models.Product>().Result;

               //System.Net.Http.Formatting.XmlMediaTypeFormatter formatter = new System.Net.Http.Formatting.XmlMediaTypeFormatter();

               //List<SubDistribution.Models.Product> products = this.Deserialize<List<SubDistribution.Models.Product>>(formatter, result);

                return Request.CreateResponse(HttpStatusCode.OK, xmlDoc);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
        }

        private T Deserialize<T>(System.Net.Http.Formatting.MediaTypeFormatter formatter, string str) where T : class
        {
            // Write the serialized string to a memory stream.
            System.IO.Stream stream = new System.IO.MemoryStream();

            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream);

            writer.Write(str);

            writer.Flush();

            stream.Position = 0;

            // Deserialize to an object of type T
            return formatter.ReadFromStreamAsync(typeof(T), stream, null, null).Result as T;
        }

        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}