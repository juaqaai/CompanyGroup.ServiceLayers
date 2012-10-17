using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    public class SalesOrderCreateResult
    {
        /// <summary>
        /// megrendelés válaszüzenet
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="message"></param>
        public SalesOrderCreateResult(int resultCode, string message)
        {
            this.ResultCode = resultCode;

            this.Message = message;
        }

        public int ResultCode { get; set; }

        public string Message { get; set; }
    }
}
