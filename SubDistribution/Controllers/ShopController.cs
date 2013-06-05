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

                //string result = response.Content.ReadAsAsync<string>().Result;

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return ThrowHttpError(ex);
            }
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