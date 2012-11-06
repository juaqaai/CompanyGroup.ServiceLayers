using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebClient.Controllers
{
    public class WebshopApiController : ApiBaseController
    {
        // GET api/webshop
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/webshop/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/webshop
        public void Post([FromBody]string value)
        {
        }

        // PUT api/webshop/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/webshop/5
        public void Delete(int id)
        {
        }
    }
}
